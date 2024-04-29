using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;
using MetricDashboard.Extensions;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;
using SharpBucket.V2;
using SharpBucket.V2.EndPoints;
using SharpBucket.V2.Pocos;
using static System.Formats.Asn1.AsnWriter;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class CodeTaskMergeCountCalculator : IMetricCalculator
    {
        /*
         * sum of all code task, merge, commit counts.
         * options: scope (for the previous 6 months, month, week, sprint)
         */
        public MetricEnum MetricEnum => MetricEnum.CODE_TASK_MERGE_COMMIT_COUNT;
        private readonly SharpBucketV2 _bitbucket;
        private readonly ILogger<Worker> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly Jira _jira;
        public CodeTaskMergeCountCalculator(ILogger<Worker> logger, BitBucketService bitBucketService, IDbContextFactory<ApplicationDbContext> dbFactory, JiraService jiraService)
        {
            _logger = logger;
            _bitbucket = bitBucketService.GetInstance();
            _dbFactory = dbFactory;
            _jira = jiraService.GetInstance();
        }
        public async Task Calculate()
        {
            try
            {
                (var user, var repos) = _bitbucket.GetCache();
                var objectsAffectingScore = new List<(string repoName, int taskCount, int pullRequestCount, int commitCount)>();
                using var _context = _dbFactory.CreateDbContext();
                var globalSettings = _context.GlobalMetricSettings.AsNoTracking().First(x => x.Id == 1);
                var scopeDateTime = globalSettings.Scope.GetDateTime(globalSettings.SprintLength);

                var projectKeys = (await _jira.Projects.GetProjectsAsync()).Select(x => x.Key.ToLower());
                foreach (var repo in repos)
                {
                    var repoResource = _bitbucket.RepositoriesEndPoint().RepositoryResource(user.display_name, repo.name);

                    var pullRequests = repoResource.PullRequestsResource()
                        .ListPullRequests(new ListPullRequestsParameters
                        {
                            States = Enum.GetValues(typeof(PullRequestState)).Cast<PullRequestState>().ToList(),
                            Filter = $"created_on > {scopeDateTime.ToString("yyyy-MM-ddTHH:mm:sszzz")}"
                        });

                    var taskCount = pullRequests.Select(x => x.source.branch.name).Where(x => projectKeys.Any(y => x.Contains(y))).Distinct().Count();

                    var commits = repoResource.ListCommits();
                    objectsAffectingScore.Add((repo.name, taskCount, pullRequests.Count, commits.Count));
                }

                await _context.MetricResults.AddAsync(new Data.Models.MetricResult()
                {
                    MetricEnum = MetricEnum,
                    TimeScope = globalSettings.Scope,
                    Score = objectsAffectingScore.Select(x => x.taskCount + x.pullRequestCount + x.commitCount).Sum(),
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