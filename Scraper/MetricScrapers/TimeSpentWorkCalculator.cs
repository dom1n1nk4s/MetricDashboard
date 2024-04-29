using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;
using MetricDashboard.Extensions;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class TimeSpentWorkCalculator : IMetricCalculator
    {
        /*
         * foreach task
         * get total sum of all subtask time spent
         * average out over all tasks
         * 
         * options: scope (for the previous 6 months, month, week, sprint);
         */
        public MetricEnum MetricEnum => MetricEnum.TIME_SPENT_WORKING;
        private readonly Jira _jira;
        private readonly ILogger<Worker> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        public TimeSpentWorkCalculator(ILogger<Worker> logger, JiraService jiraService, IDbContextFactory<ApplicationDbContext> dbFactory)
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
                var globalSettings = _context.GlobalMetricSettings.AsNoTracking().First(x => x.Id == 1);
                var issues = _jira.GetCachedIssues(globalSettings);
                var objectsAffectingScore = new List<(string issueKey, double countOfHoursPerTask)>();
                foreach (var issue in issues)
                {
                    var hoursPerTask = (await issue.GetSubTasksAsync()).AsEnumerable()
                        .Select(x => (x.TimeTrackingData?.TimeSpentInSeconds ?? default) / (60.0 * 60.0)).Sum(); // to hours
                    objectsAffectingScore.Add((issue.Key.Value, hoursPerTask));
                }
                await _context.MetricResults.AddAsync(new Data.Models.MetricResult()
                {
                    MetricEnum = MetricEnum,
                    TimeScope = globalSettings.Scope,
                    Score = objectsAffectingScore.Average(x => x.countOfHoursPerTask),
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