using Atlassian.Jira;
using MetricDashboard.Components.Account.Pages.Manage;
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
        public CodeTaskMergeCountCalculator(ILogger<Worker> logger, BitBucketService bitBucketService, IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _logger = logger;
            _bitbucket = bitBucketService.GetInstance();
            _dbFactory = dbFactory;
        }
        public async Task Calculate()
        {
            (var user, var repos) = _bitbucket.GetCache();
            var objectsAffectingScore = new List<(string repoName, int taskCount, int mergeCount, int commitCount)>();
            //dont forget to apply scope...
            foreach(var repo in repos)
            {
                var repoResource = _bitbucket.RepositoriesEndPoint().RepositoryResource(user.display_name, repo.name);
                var taskCount = repoResource.PullRequestsResource()
                    .ListPullRequests(new ListPullRequestsParameters
                    { States = Enum.GetValues(typeof(PullRequestState)).Cast<PullRequestState>().ToList() });// IMPOSSIBLE THROUGH BITBUCKET API, need to either 1.parse title 2.get through jira api
                var mergeCount = repoResource.PullRequestsResource()
                    .ListPullRequests( new ListPullRequestsParameters
                    { States = Enum.GetValues(typeof(PullRequestState)).Cast<PullRequestState>().ToList()}).Count; 
                var commitCount = repoResource.ListCommits().Count;
                objectsAffectingScore.Add((repo.name,taskCount.Count,mergeCount,commitCount));
            }

        }
    }
}