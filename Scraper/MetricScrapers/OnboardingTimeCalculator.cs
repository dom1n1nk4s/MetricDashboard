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
    public class OnboardingTimeCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.ONBOARDING_TIME;

        private readonly JiraService _jiraService;
        private readonly ILogger<OnboardingTimeCalculator> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        public OnboardingTimeCalculator(ILogger<OnboardingTimeCalculator> logger, JiraService jiraService, IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _logger = logger;
            _jiraService = jiraService;
            _dbFactory = dbFactory;
        }
        public async Task Calculate()
        {
            try
            {
                using var _context = _dbFactory.CreateDbContext();
                var globalSettings = await _context.GlobalMetricSettings.FirstAsync(x => x.Id == 1);
                var settings = (await _context.Metrics.AsNoTracking().FirstAsync(x => x.MetricEnum == MetricEnum))?.Settings?.Deserialize<OnboardingTimeSettings>();
                if (settings == null)
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
                var issue = await _jiraService.GetIssueAsync(taskId);
                if (issue == null)
                {
                    _logger.LogError($"Failed to get issue for {nameof(OnboardingTimeCalculator)}");
                    return;
                }
                var eachPersonsTimeSpent = (await _jiraService.GetWorklogsAsync(issue)).GroupBy(x => x.AuthorUser.Email).Select(x => (x.Key, x.Select(z => z.TimeSpentInSeconds).Sum() / (60.0*60*24))); //to days
                var average = eachPersonsTimeSpent.Any() ? eachPersonsTimeSpent.Select(x => x.Item2).Average() : 0;


                await _context.MetricResults.AddAsync(new Data.Models.MetricResult()
                {
                    MetricEnum = MetricEnum,
                    TimeScope = globalSettings.Scope,
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