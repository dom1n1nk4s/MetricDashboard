using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;
using MetricDashboard.Extensions;
using MetricDashboard.Models;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;
using SharpBucket;
using SharpBucket.V2;
using SharpBucket.V2.Pocos;

namespace MetricDashboard.Scraper.MetricScrapers
{
    public class LeadTimeCalculator : IMetricCalculator
    {
        /*
         * var deployments = get all deployments for scope
         * foreach tasks.Where(x => x.status == done)
         * var taskDeploymentTime = task.RelatedPr.RelatedDeployment
         * result.add (taskDeploymentTime - task.CreationTime)
         * 
         * options: scope (for the previous 6 months, month, week, sprint);
         */
        public MetricEnum MetricEnum => MetricEnum.LEAD_TIME_FOR_CHANGES;
        private readonly ILogger<LeadTimeCalculator> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly JiraService _jiraService;
        private readonly BitBucketService _bitbucketService;

        public LeadTimeCalculator(ILogger<LeadTimeCalculator> logger, BitBucketService bitBucketService, IDbContextFactory<ApplicationDbContext> dbFactory, JiraService jiraService)
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
                using var _context = _dbFactory.CreateDbContext();
                (var user, var repos) = _bitbucketService.GetCache();
                var globalSettings = _context.GlobalMetricSettings.AsNoTracking().First(x => x.Id == 1);
                var issues = _jiraService.GetCachedIssues(globalSettings);
                var workspace = _bitbucketService.GetWorkspace();
                var scopeDateTime = globalSettings.Scope.GetDateTime(globalSettings.SprintLength);
                var prodDeploymentDatesForRepositories = new Dictionary<string, List<DateTime?>>();
                var objectsAffectingScore = new List<(string issueKey, double days)>();
                foreach (var repo in repos)
                {
                    var environments = await _bitbucketService.ListEnvironmentsAsync(user.display_name,repo.name);
                    var prodEnvironmentUUID = environments.First(x => x.name.ToLower().Contains("prod")).uuid;
                    var deployments = await _bitbucketService.GetAsync<DeploymentResponse>
                        ($"/repositories/{workspace.slug}/{repo.slug}/deployments?q=self.state.completed_on > {scopeDateTime.ToString("yyyy-MM-ddTHH:mm:00zzz")}",
                        new CancellationToken());

                    var prodDeployments = deployments.Values.Where(x => x.State.Name.ToUpper() == "COMPLETED" && x.Environment.Uuid == prodEnvironmentUUID)
                        .ToList();
                    if (!prodDeployments?.Any() ?? true)
                    {
                        continue;
                    }
                    var dates = new List<DateTime?>();
                    foreach (var prodDeployment in prodDeployments!)
                    {
                        dates.Add(prodDeployment.State.CompletedOn);
                    }
                    prodDeploymentDatesForRepositories.Add(repo.slug, dates);
                }
                foreach (var issue in issues.Where(x => x.Status.Name.ToLower() == "done" && !x.Type.IsSubTask))
                {
                    var response = await _jiraService.ExecuteRequestAsync<PullRequestResponse>(RestSharp.Method.GET,
                        $"/rest/dev-status/1.0/issue/detail?issueId={issue.JiraIdentifier}&applicationType=bitbucket&dataType=pullrequest");
                    var pullRequest = response.Detail.First().PullRequests.Where(x => x.Status.ToLower() == "merged").MaxByOrDefault(x => x.LastUpdate);
                    if (pullRequest == null)
                    {
                        continue;
                    }
                    var nearestDeployment = prodDeploymentDatesForRepositories[pullRequest.RepositoryName].Where(x => x > pullRequest.LastUpdate).MinByOrDefault(x=>x);
                    if (nearestDeployment != null)
                    {
                        objectsAffectingScore.Add((issue.Key.Value, (nearestDeployment.Value - issue.Created!.Value).TotalMinutes / (60 * 24))); //to days
                    }
                }
                await _context.MetricResults.AddAsync(new Data.Models.MetricResult()
                {
                    MetricEnum = MetricEnum,
                    TimeScope = globalSettings.Scope,
                    Score = objectsAffectingScore.Any() ? objectsAffectingScore.Select(x => x.days).Average() : 0,
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