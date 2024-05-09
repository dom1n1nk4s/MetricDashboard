using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;
using MetricDashboard.Extensions;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;
using static System.TimeZoneInfo;

namespace MetricDashboard.Scraper.MetricScrapers
{
    public class WorkflowIntCalculator : IMetricCalculator
    {
        /*
         * foreach issue
         * foreach statusTransition.where(previousStatus == onHold)
         * get time spent being on hold
         * 
         * average out time
         * 
         * options: scope (for the previous 6 months, month, week, sprint);
         */
        public MetricEnum MetricEnum => MetricEnum.WORKFLOW_INTERRUPTION_TIME;
        private readonly JiraService _jiraService;
        private readonly ILogger<WorkflowIntCalculator> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        public WorkflowIntCalculator(ILogger<WorkflowIntCalculator> logger, JiraService jiraService, IDbContextFactory<ApplicationDbContext> dbFactory)
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
                var globalSettings = _context.GlobalMetricSettings.AsNoTracking().First(x => x.Id == 1);
                var objectsAffectingScore = new List<(string issueKey, double durationOnHoldInHours)>();
                var issues = _jiraService.GetCachedIssues(globalSettings);
                foreach (var issue in issues.Where(x => !x.Type.IsSubTask))
                {
                    var changeLogs = (await _jiraService.GetChangeLogsAsync(issue)).OrderBy(x=>x.CreatedDate);
                    var onHoldChanges = changeLogs.Where(x => x.Items.Any(y => y.ToValue.ToLower() == "on hold"));
                    var totalTimeOnHold = new TimeSpan();
                    foreach (var onHoldChange in onHoldChanges)
                    {
                        var nextTransition = changeLogs.FirstOrDefault(x => x.CreatedDate > onHoldChange.CreatedDate &&
                            x.Items.Any(item => item.FieldName.Equals("status", StringComparison.OrdinalIgnoreCase)));
                        if (nextTransition != null)
                        {
                            var durationOnHold = nextTransition.CreatedDate - onHoldChange.CreatedDate;
                            totalTimeOnHold += durationOnHold;
                        }
                    }

                    objectsAffectingScore.Add((issue.Key.Value, totalTimeOnHold.TotalHours));
                }

                await _context.MetricResults.AddAsync(new Data.Models.MetricResult()
                {
                    MetricEnum = MetricEnum,
                    TimeScope = globalSettings.Scope,
                    Score = objectsAffectingScore.Average(x => x.durationOnHoldInHours),
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