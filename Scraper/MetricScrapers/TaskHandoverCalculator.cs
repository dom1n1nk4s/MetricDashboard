using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;
using MetricDashboard.Extensions;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class TaskHandoverCalculator : IMetricCalculator
    {
        /*
         * foreach task
         * get all subtasks where words are not "Analysis, Test,"
         * results.add(select unique assignees count)
         * result average out
         * 
         * 
         * options: scope (for the previous 6 months, month, week, sprint);
         */
        public MetricEnum MetricEnum => MetricEnum.TASK_HANDOVERS_BEFORE_COMPLETION;
        private readonly Jira _jira;
        private readonly ILogger<Worker> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        public TaskHandoverCalculator(ILogger<Worker> logger, JiraService jiraService, IDbContextFactory<ApplicationDbContext> dbFactory)
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
                var objectsAffectingScore = new List<(string issueKey, int countOfPeopleWorking)>();
                var notProgrammingRelatedTasks = new string[] { "analysis", "test", "analyse", "analyze" };
                foreach (var issue in issues)
                {
                    var countOfPeopleWorking = (await issue.GetSubTasksAsync()).AsEnumerable()
                        .Where(x => notProgrammingRelatedTasks.Any(y => x.Summary.ToLower().Contains(y))).Select(x => x.Assignee).Distinct().Count();
                    objectsAffectingScore.Add((issue.Key.Value, countOfPeopleWorking));
                }
                await _context.MetricResults.AddAsync(new Data.Models.MetricResult()
                {
                    MetricEnum = MetricEnum,
                    Score = objectsAffectingScore.Average(x => x.countOfPeopleWorking),
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