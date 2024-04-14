using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;
using MetricDashboard.Extensions;
using MetricDashboard.Models;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class WorkerRetRateCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.WORKER_RETENTION_RATE;

        private readonly Jira _jira;
        private readonly ILogger<Worker> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        public WorkerRetRateCalculator(ILogger<Worker> logger, JiraService jiraService, IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _logger = logger;
            _jira = jiraService.GetInstance();
            _dbFactory = dbFactory;
        }
        public async Task Calculate()
        {
            try
            {

            using var _context = _dbFactory.CreateDbContext();
                 var users = (await  _jira.RestClient.ExecuteRequestAsync<List<JiraUser>>(RestSharp.Method.Get, "/rest/api/3/users/search")).Where(x=>x.Email != null);
                // for active users -> find first issue where they are a watcher, get its datetime and subtract from datetime.now
                // for inactive users -> find first issue where they are a watcher, get its datetime and subtract from the last issue they were a watcher.
                //this might be really laggy and take a lot of resources, but its just a PoC
            var settings = (await _context.Metrics.AsNoTracking().FirstAsync(x => x.MetricEnum == MetricEnum))?.Settings?.Deserialize<OnboardingTimeSettings>();
            if (settings == null)
            {
                _logger.LogError($"Failed to get settings for {nameof(WorkerRetRateCalculator)}");
                return;
            }
            var taskId = settings.OnboardingTaskId;
            if (string.IsNullOrWhiteSpace(taskId))
            {
                _logger.LogError($"Failed to get issue for {nameof(WorkerRetRateCalculator)}. Issue is invalid.");
                return;
            }
            var issue = await _jira.Issues.GetIssueAsync(taskId);
            if (issue == null)
            {
                _logger.LogError($"Failed to get issue for {nameof(WorkerRetRateCalculator)}");
                return;
            }
            var eachPersonsTimeSpent = (await issue.GetWorklogsAsync()).GroupBy(x => x.AuthorUser.AccountId).Select(x => (x.Key, x.Select(z => z.TimeSpentInSeconds).Sum()));
            var average = eachPersonsTimeSpent.Select(x => x.Item2).Average();


            await _context.MetricResults.AddAsync(new Data.Models.MetricResult()
            {
                MetricEnum = MetricEnum,
                Score = average,
                ObjectsAffectingScore = eachPersonsTimeSpent.Serialize()
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