using Atlassian.Jira;
using MetricDashboard.Data.Models;

namespace MetricDashboard.Extensions
{
    public static class JiraExtensions
    {
        private static List<Issue> cachedIssues = new List<Issue>();
        private static DateTime cachedTime = DateTime.MinValue;
        public static List<Issue> GetCachedIssues(this Jira jira, GlobalMetricSettings settings, bool forceRefresh = false)
        {
            if((DateTime.Now - cachedTime).TotalMinutes > 60 || forceRefresh)
            {
                var lastAllowedDateTime = settings.Scope.GetDateTime(settings.SprintLength);
                cachedIssues = jira.Issues.Queryable.Where(x => x.Created > lastAllowedDateTime).ToList();
                cachedTime = DateTime.Now;
            }
            return cachedIssues;
        }
    }
}
