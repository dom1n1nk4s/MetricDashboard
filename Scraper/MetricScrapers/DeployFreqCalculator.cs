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
    internal class DeployFreqCalculator : IMetricCalculator
    {
        /*
         * get total deployment count for scope
         * options: scope (for the previous 6 months, month, week, sprint)
         */
        public MetricEnum MetricEnum => MetricEnum.DEPLOYMENT_FREQUENCY;
        private readonly SharpBucketV2 _bitbucket;
        private readonly ILogger<Worker> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public DeployFreqCalculator(BitBucketService bitbucket, ILogger<Worker> logger, IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _bitbucket = bitbucket.GetInstance();
            _logger = logger;
            _dbFactory = dbFactory;
        }

        public async Task Calculate()
        {
            try
            {
                using var _context = _dbFactory.CreateDbContext();
                (var user, var repos) = _bitbucket.GetCache();
                var globalSettings = _context.GlobalMetricSettings.AsNoTracking().First(x => x.Id == 1);
                var workspace = _bitbucket.WorkspacesEndPoint().ListWorkspaces().First(); //TODO: MOVE TO OPTIONS AS PRIMARY WORKSPACE (low priority)
                var scopeDateTime = globalSettings.Scope.GetDateTime(globalSettings.SprintLength);
                var prodCommitsForRepositories = new Dictionary<string, List<DateTime?>>();
                var objectsAffectingScore = new List<(string repoKey, double deploymentCount)>();
                foreach (var repo in repos)
                {
                    var repoResource = _bitbucket.RepositoriesEndPoint().RepositoryResource(user.display_name, repo.name);
                    var environments = await repoResource.EnvironmentsResource.ListEnvironmentsAsync();
                    var prodEnvironmentUUID = environments.First(x => x.name.ToLower().Contains("prod")).uuid;
                    var deployments = await _bitbucket.GetAsync<DeploymentResponse>
                        ($"/repositories/{workspace.slug}/{repo.slug}/deployments?q=self.state.completed_on > {scopeDateTime.ToString("yyyy-MM-ddTHH:mm:sszzz")}",
                        new CancellationToken());

                    var prodDeployments = deployments.Values.Where(x => x.State.Name == "COMPLETED" && x.Environment.Uuid == prodEnvironmentUUID)
                        .ToList();
                    if (!prodDeployments?.Any() ?? true)
                    {
                        objectsAffectingScore.Add((repo.slug, 0));
                    }
                    else
                    {
                        objectsAffectingScore.Add((repo.slug, prodDeployments.Count));
                    }

                }
                await _context.MetricResults.AddAsync(new Data.Models.MetricResult()
                {
                    MetricEnum = MetricEnum,
                    Score = objectsAffectingScore.Select(x => x.deploymentCount).Average(),
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