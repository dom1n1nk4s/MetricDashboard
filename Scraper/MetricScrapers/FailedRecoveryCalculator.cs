using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;
using MetricDashboard.Extensions;
using MetricDashboard.Models;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;
using SharpBucket;
using SharpBucket.V2;

namespace MetricDashboard.Scraper.MetricScrapers
{
    public class FailedRecoveryCalculator : IMetricCalculator
    {
        /*
         * var critIncidentDeployments = Tasks.where(type is critical incident).select(inc => inc.RelatedPR.RelatedDeployment);
         * foreach critIncidentDeployments
         * var fixDeployment = find next deployment of same project
         * result.add( fixDeployment.DateTime - critIncidentDeployments.DateTime)
         * result average out
         * 
         * options: scope (for the previous 6 months, month, week, sprint); globalopt: critical incident type (optional)
         */
        public MetricEnum MetricEnum => MetricEnum.FAILED_DEPLOYMENT_RECOVERY_TIME;
        private readonly ILogger<FailedRecoveryCalculator> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly JiraService _jiraService;
        private readonly BitBucketService _bitbucketService;
        public FailedRecoveryCalculator(ILogger<FailedRecoveryCalculator> logger, BitBucketService bitBucketService, IDbContextFactory<ApplicationDbContext> dbFactory, JiraService jiraService)
        {
            _logger = logger;
            _bitbucketService = bitBucketService;
            _dbFactory = dbFactory;
            _jiraService = jiraService;
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
                var objectsAffectingScore = new List<(string repoKey, double hoursToFix)>();
                var criticalIssues = issues.Where(x => x.Type.Name.ToLower() == "critical" && x.Status.Name.ToLower() == "done");
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
                var repoDeploymentFixTimes = new Dictionary<string, List<double>>();
                foreach (var issue in criticalIssues)
                {
                    var response = await _jiraService.ExecuteRequestAsync<PullRequestResponse>(RestSharp.Method.GET,
                        $"/rest/dev-status/1.0/issue/detail?issueId={issue.JiraIdentifier}&applicationType=bitbucket&dataType=pullrequest");
                    var pullRequest = response.Detail.First().PullRequests.Where(x => x.Status.ToLower() == "merged").MaxByOrDefault(x => x.LastUpdate);
                    if (pullRequest == null)
                    {
                        continue;
                    }
                    var deploymentWhichCausedIssue = prodDeploymentDatesForRepositories[pullRequest.RepositoryName].Where(x => x < pullRequest.LastUpdate).MaxByOrDefault(x => x);
                    var deploymentWhichFixedIssue = prodDeploymentDatesForRepositories[pullRequest.RepositoryName].Where(x => x > pullRequest.LastUpdate).MinByOrDefault(x => x);
                    if (deploymentWhichCausedIssue != null && deploymentWhichFixedIssue != null)
                    {
                        if (!repoDeploymentFixTimes.ContainsKey(pullRequest.RepositoryName))
                        {
                            repoDeploymentFixTimes.Add(pullRequest.RepositoryName, new List<double>());
                        }
                        repoDeploymentFixTimes[pullRequest.RepositoryName].Add((deploymentWhichFixedIssue.Value - deploymentWhichCausedIssue.Value).TotalMinutes / (60)); //to hours
                    }
                }

                objectsAffectingScore = repoDeploymentFixTimes.Select(x => (x.Key, x.Value.Any() ? x.Value.Average() : 0)).ToList();
                await _context.MetricResults.AddAsync(new Data.Models.MetricResult()
                {
                    MetricEnum = MetricEnum,
                    TimeScope = globalSettings.Scope,
                    Score = objectsAffectingScore.Any() ? objectsAffectingScore.Select(x => x.hoursToFix).Average() : 0,
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