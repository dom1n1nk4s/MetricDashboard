using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;
using MetricDashboard.Extensions;
using MetricDashboard.Models;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class OnboardingTimeCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.ONBOARDING_TIME;

        private readonly Jira _jira;
        private readonly ILogger<Worker> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        public OnboardingTimeCalculator(ILogger<Worker> logger, JiraService jiraService, IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _logger = logger;
            _jira = jiraService.GetInstance();
            _dbFactory = dbFactory;
        }
        public void Calculate()
        {
            using var dbContext = _dbFactory.CreateDbContext();
            var settings = dbContext.Metrics.AsNoTracking().First(x => x.MetricEnum == Data.Enums.MetricEnum.ONBOARDING_TIME)?.Settings?.Deserialize<OnboardingTimeSettings>();
            if(settings == null)
            {
                _logger.LogError($"Failed to get settings for {nameof(OnboardingTimeCalculator)}");
                return;
            }
            var taskId = settings.OnboardingTaskId;
            if (string.IsNullOrWhiteSpace(taskId))
            {
                _logger.LogError($"Failed to get issue for {nameof(OnboardingTimeCalculator)}. Issue is invalid.");
                return;
            }
            var issue = _jira.Issues.GetIssueAsync(taskId).GetAwaiter().GetResult();
            if(issue == null)
            {
                _logger.LogError($"Failed to get issue for {nameof(OnboardingTimeCalculator)}");
                return;
            }
            var eachPersonsTimeSpent = issue.GetWorklogsAsync().Result.GroupBy(x => x.AuthorUser.AccountId).Select(x => (x.Key, x.Select(z => z.TimeSpentInSeconds).Sum()));
            var average = eachPersonsTimeSpent.Select(x => x.Item2).Average();

            //save metricCalculation values as eachPersonsTimeSpent 
            //save score 
        }

    }
}