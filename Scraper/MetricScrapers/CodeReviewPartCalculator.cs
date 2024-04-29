using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;
using MetricDashboard.Extensions;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;
using SharpBucket.V2;
using SharpBucket.V2.EndPoints;
using SharpBucket.V2.Pocos;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class CodeReviewPartCalculator : IMetricCalculator
    {
        /*
         * 
         * foreach PR
         * var reviewers = (get reviewers assigned to pr)
         * result.add( reviewers.Count(x => x.APPROVED_OR_LEFT_COMMENT) / reviewers.Count())
         * 
         * 
         * options: scope (for the previous 6 months, month, week, sprint)
         */
        public MetricEnum MetricEnum => MetricEnum.CODE_REVIEW_PARTICIPATION;
        private readonly SharpBucketV2 _bitbucket;
        private readonly ILogger<Worker> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public CodeReviewPartCalculator(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<Worker> logger, BitBucketService bitbucket)
        {
            _dbFactory = dbFactory;
            _logger = logger;
            _bitbucket = bitbucket.GetInstance();
        }

        public async Task Calculate()
        {
            try
            {
                (var user, var repos) = _bitbucket.GetCache();
                var objectsAffectingScore = new List<(string pullRequestName, double participationPercent)>();
                using var _context = _dbFactory.CreateDbContext();
                var globalSettings = _context.GlobalMetricSettings.AsNoTracking().First(x => x.Id == 1);
                var scopeDateTime = globalSettings.Scope.GetDateTime(globalSettings.SprintLength);

                foreach (var repo in repos)
                {
                    var repoResource = _bitbucket.RepositoriesEndPoint().RepositoryResource(user.display_name, repo.name);

                    var pullRequests = repoResource.PullRequestsResource()
                        .ListPullRequests(new ListPullRequestsParameters
                        {
                            States = Enum.GetValues(typeof(PullRequestState)).Cast<PullRequestState>().ToList(),
                            Filter = $"created_on > {scopeDateTime.ToString("yyyy-MM-ddTHH:mm:sszzz")}"
                        });
                    foreach (var stubPullRequest in pullRequests)
                    {
                        var pullRequest =await repoResource.PullRequestsResource().PullRequestResource(stubPullRequest.id.Value).GetPullRequestAsync();
                        var activities = repoResource.PullRequestsResource().PullRequestResource(pullRequest.id.Value).ListPullRequestActivities();
                        var uniqueApprovals = activities.Where(x => x.approval != null).Select(x => x.approval.user.account_id).ToList();
                        var uniqueComments = activities.Where(x => x.comment != null).Select(x => x.comment.user.account_id).ToList();

                        var activeParticipantCount = uniqueApprovals.Concat(uniqueComments).Distinct().Count() * 1.0;
                        objectsAffectingScore.Add((pullRequest.title, 100*activeParticipantCount / Math.Max(pullRequest?.reviewers?.Count ?? 1, 1)));
                    }
                }

                await _context.MetricResults.AddAsync(new Data.Models.MetricResult()
                {
                    MetricEnum = MetricEnum,
                    TimeScope = globalSettings.Scope,
                    Score = objectsAffectingScore.Any() ? objectsAffectingScore.Select(x => x.participationPercent).Average() : 0,
                    ObjectsAffectingScore = objectsAffectingScore.Serialize()
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}