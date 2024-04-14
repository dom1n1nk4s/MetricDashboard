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
        public async Task Calculate()
        {
            using var _context = _dbFactory.CreateDbContext();
            var settings = (await _context.Metrics.AsNoTracking().FirstAsync(x => x.MetricEnum == MetricEnum))?.Settings?.Deserialize<OnboardingTimeSettings>();
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
            var issue = await _jira.Issues.GetIssueAsync(taskId);
            if(issue == null)
            {
                _logger.LogError($"Failed to get issue for {nameof(OnboardingTimeCalculator)}");
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

    }
}