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
                var globalSettings = await _context.GlobalMetricSettings.FirstAsync(x => x.Id == 1);
                var users = (await _jira.RestClient.ExecuteRequestAsync<List<JiraUser>>(RestSharp.Method.GET, "/rest/api/3/users/search")).Where(x => x.Locale != null).ToList();
                var objectsAffectingScore = new List<(string Name, DateTime ActiveFrom, DateTime ActiveTo)>();
                foreach (var user in users)
                {
                    var issues = (await _jira.Issues.GetIssuesFromJqlAsync($"reporter = \"{user.AccountId}\" OR assignee = \"{user.AccountId}\"", maxIssues: 10000))
                        .Select(x => x.Created).ToList();
                    if(!issues.Any())
                    {
                        continue;
                    }
                    var firstIssue = issues.Min();
                    var lastIssue = issues.Max();
                    objectsAffectingScore.Add(new(user.DisplayName, firstIssue.Value, lastIssue.Value));
                }

                var score = objectsAffectingScore.Any() ? objectsAffectingScore.Select(x => CalculateMonthDifference(x.ActiveFrom, x.ActiveTo)).Average() : 0;

                await _context.MetricResults.AddAsync(new Data.Models.MetricResult()
                {
                    MetricEnum = MetricEnum,
                    TimeScope = globalSettings.Scope,
                    Score = score,
                    ObjectsAffectingScore = objectsAffectingScore.Serialize()
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        private double CalculateMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month;
            double daysInMonth = 0;

            if (startDate.Day > endDate.Day)
            {
                daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            }
            else
            {
                daysInMonth = DateTime.DaysInMonth(endDate.Year, endDate.Month);
            }

            double daysDifference = (endDate - startDate).TotalDays;
            double monthsDifference = monthsApart + (daysDifference / daysInMonth);

            return monthsDifference;
        }
    }
}