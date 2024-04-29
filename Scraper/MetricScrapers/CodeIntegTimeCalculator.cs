using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;
using MetricDashboard.Extensions;
using MetricDashboard.Models;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;
using SharpBucket.V2;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class CodeIntegTimeCalculator : IMetricCalculator
    {
        /*
         * foreach issue
         * 
         * results.add(issue.pullrequest.mergeTime - issue.CreationTime)
         * 
         * average results out. output as days
         * 
         * options: scope (for the previous 6 months, month, week, sprint)
         */
        public MetricEnum MetricEnum => MetricEnum.CODE_INTEGRATION_TIME;
        private readonly ILogger<Worker> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly Jira _jira;

        public CodeIntegTimeCalculator(JiraService jira, IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<Worker> logger)
        {
            _jira = jira.GetInstance();
            _dbFactory = dbFactory;
            _logger = logger;
        }

        public async Task Calculate()
        {
            try {
                using var _context = _dbFactory.CreateDbContext();
                var globalSettings = _context.GlobalMetricSettings.AsNoTracking().First(x => x.Id == 1);
                var issues = _jira.GetCachedIssues(globalSettings);
                var objectsAffectingScore = new List<(string issueKey, double hours)>();

                foreach (var issue in issues.Where(x => x.Status.Name == "Done" && !x.Type.IsSubTask))
                {
                    var response = await _jira.RestClient.ExecuteRequestAsync<PullRequestResponse>(RestSharp.Method.GET,
                        $"/rest/dev-status/1.0/issue/detail?issueId={issue.JiraIdentifier}&applicationType=bitbucket&dataType=pullrequest");
                    var pullRequest = response.Detail.First().PullRequests.Where(x => x.Status.ToLower() == "merged").MaxByOrDefault(x => x.LastUpdate);
                    if (pullRequest == null)
                    {
                        continue;
                    }
                    objectsAffectingScore.Add((issue.Key.Value, (pullRequest.LastUpdate - issue.Created!.Value).TotalMinutes / 60)); //to hours
                }
                await _context.MetricResults.AddAsync(new Data.Models.MetricResult()
                {
                    MetricEnum = MetricEnum,
                    TimeScope = globalSettings.Scope,
                    Score = objectsAffectingScore.Any() ? objectsAffectingScore.Select(x => x.hours).Average() : 0,
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