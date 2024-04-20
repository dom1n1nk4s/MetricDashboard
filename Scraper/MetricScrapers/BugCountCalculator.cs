using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;
using MetricDashboard.Extensions;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class BugCountCalculator : IMetricCalculator
    {
        /*
         * select average bug count per last scope.
         * options: scope (default)
         */
        public MetricEnum MetricEnum => MetricEnum.BUG_COUNT;
        private readonly Jira _jira;
        private readonly ILogger<Worker> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        public BugCountCalculator(ILogger<Worker> logger, JiraService jiraService, IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _logger = logger;
            _jira = jiraService.GetInstance();
            _dbFactory = dbFactory;
        }
        public async Task Calculate()
        {
            using var _context = _dbFactory.CreateDbContext();
            var globalSettings = _context.GlobalMetricSettings.AsNoTracking().First(x => x.Id == 1);
            var issues = _jira.GetCachedIssues(globalSettings);
            var objectsAffectingScore = new List<(string name, DateTime dateTime)>();

            objectsAffectingScore = issues.Where(x => !x.Type.IsSubTask && x.Type.Name == "Bug")
                .Select<Issue, (string name, DateTime dateTime)>(x => (x.Summary, x.Created.Value)).ToList();

            await _context.MetricResults.AddAsync(new Data.Models.MetricResult()
            {
                MetricEnum = MetricEnum,
                Score = objectsAffectingScore.Count(),
                ObjectsAffectingScore = objectsAffectingScore.Serialize()
            });
            await _context.SaveChangesAsync();
        }
    }
}