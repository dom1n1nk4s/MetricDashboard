using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;
using MetricDashboard.Extensions;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class BusinessValuePercCalculator : IMetricCalculator
    {
        /*
         * foreach task: filter out each substask containing words "PR, Code review, Analysis, etc.", then get all timesheet allocated hours on the subtasks which are left.
         * calculate the sum(LeftoverTasks) and divide from total time spent on this task (perhaps a sum of all subtasks including filtered).
         * then average through all of tasks.
         * 
         * options: scope (for the previous 6 months, month, week, sprint)
         */
        public MetricEnum MetricEnum => MetricEnum.BUSINESS_VALUE_PERCENTAGE;
        private readonly Jira _jira;
        private readonly ILogger<Worker> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        public BusinessValuePercCalculator(ILogger<Worker> logger, JiraService jiraService, IDbContextFactory<ApplicationDbContext> dbFactory)
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
                var objectsAffectingScore = new List<(string issueKey, double countOfHoursWorking, double countOfTotalHours)>();
                var notProgrammingRelatedTasks = new string[] { "analysis", "test", "analyse", "analyze", "code review", "PR", "pull request", "merge request" };
                foreach (var issue in issues)
                {
                    var subtasks = (await issue.GetSubTasksAsync()).ToList();
                    var hoursSpentWorking = subtasks
                        .Where(x => !notProgrammingRelatedTasks.Any(y => x.Summary.ToLower().Contains(y)))
                        .Select(x => x.TimeTrackingData.TimeSpentInSeconds.Value / (60.0 * 60.0)).Sum(); // to hours
                    var countOfTotalHours = subtasks //TODO: FIX TIMETRACKING NULL HERE
                        .Select(x => x.TimeTrackingData.TimeSpentInSeconds.Value / (60.0 * 60.0)).Sum(); // to hours
                    objectsAffectingScore.Add((issue.Key.Value, hoursSpentWorking, countOfTotalHours));
                }
                await _context.MetricResults.AddAsync(new Data.Models.MetricResult()
                {
                    MetricEnum = MetricEnum,
                    Score = objectsAffectingScore.Average(x => x.countOfHoursWorking),
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