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
    internal class WorkflowIntCalculator : IMetricCalculator
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
        private readonly Jira _jira;
        private readonly ILogger<Worker> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        public WorkflowIntCalculator(ILogger<Worker> logger, JiraService jiraService, IDbContextFactory<ApplicationDbContext> dbFactory)
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
                var objectsAffectingScore = new List<(string issueKey, double durationOnHoldInHours)>();
                var issues = _jira.Issues.Queryable.Where(x => x.Created > globalSettings.Scope.GetDateTime(globalSettings.SprintLength)).ToList();
                foreach (var issue in issues)
                {
                    var changeLogs = await issue.GetChangeLogsAsync(); //might need to order by .CreatedDate, check on debugger
                    var onHoldChanges = changeLogs.Where(x => x.Items.Any(y => y.FromValue == "On hold"));
                    var totalTimeOnHold = new TimeSpan();
                    foreach (var onHoldChange in onHoldChanges)
                    {
                        var nextTransition = changeLogs.FirstOrDefault(x => x.CreatedDate > onHoldChange.CreatedDate && x.Items.Any(item => item.FieldName.Equals("status", StringComparison.OrdinalIgnoreCase)));
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