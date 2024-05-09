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
    public class CodeTaskMergeCountCalculator : IMetricCalculator
    {
        /*
         * sum of all code task, merge, commit counts.
         * options: scope (for the previous 6 months, month, week, sprint)
         */
        public MetricEnum MetricEnum => MetricEnum.CODE_TASK_MERGE_COMMIT_COUNT;
        private readonly ILogger<CodeTaskMergeCountCalculator> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly JiraService _jiraService;
        private readonly BitBucketService _bitbucketService;
        public CodeTaskMergeCountCalculator(ILogger<CodeTaskMergeCountCalculator> logger, BitBucketService bitBucketService, IDbContextFactory<ApplicationDbContext> dbFactory, JiraService jiraService)
        {
            _logger = logger;
            _dbFactory = dbFactory;
            _jiraService = jiraService;
            _bitbucketService = bitBucketService;
        }
        public async Task Calculate()
        {
            try
            {
                (var user, var repos) = _bitbucketService.GetCache();
                var objectsAffectingScore = new List<(string repoName, int taskCount, int pullRequestCount, int commitCount)>();
                using var _context = _dbFactory.CreateDbContext();
                var globalSettings = _context.GlobalMetricSettings.AsNoTracking().First(x => x.Id == 1);
                var scopeDateTime = globalSettings.Scope.GetDateTime(globalSettings.SprintLength);

                var projectKeys = (await _jiraService.GetProjectsAsync()).Select(x => x.Key.ToLower());
                foreach (var repo in repos)
                {
                    var pullRequests = _bitbucketService
                        .ListPullRequests(user.display_name, repo.name,new ListPullRequestsParameters
                        {
                            States = Enum.GetValues(typeof(PullRequestState)).Cast<PullRequestState>().ToList(),
                            Filter = $"created_on > {scopeDateTime.ToString("yyyy-MM-ddTHH:mm:00zzz")}"
                        });

                    var taskCount = pullRequests.Select(x => x.source.branch.name).Where(x => projectKeys.Any(y => x.ToLower().Contains(y))).Distinct().Count();

                    var commits = _bitbucketService.ListCommits(user.display_name, repo.name);
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