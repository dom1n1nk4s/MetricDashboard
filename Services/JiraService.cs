using Atlassian.Jira;
using MetricDashboard.Data.Models;
using MetricDashboard.Extensions;
using MetricDashboard.Models;
using Microsoft.Extensions.Options;
using RestSharp;

namespace MetricDashboard.Services
{
    public class JiraService
    {
        private readonly Jira _jiraInstance;
        private List<Issue> cachedIssues = new List<Issue>();
        private DateTime cachedTime = DateTime.MinValue;
        public JiraService(IOptions<AppSettings> options)
        {
            var settings = options.Value;
            _jiraInstance = Jira.CreateRestClient(settings.JiraUrl, settings.JiraUsername, settings.JiraPassword);

        }
        public virtual Jira GetInstance() { return _jiraInstance; }
        public virtual List<Issue> GetCachedIssues(GlobalMetricSettings settings, bool forceRefresh = false)
        {
            if ((DateTime.Now - cachedTime).TotalMinutes > 60 || forceRefresh)
            {

                var lastAllowedDateTime = (settings?.Scope ?? Data.Enums.TimeScopeEnum.SIX_MONTHS).GetDateTime(settings?.SprintLength?? 0);
                cachedIssues = _jiraInstance.Issues.Queryable.Where(x => x.Created > lastAllowedDateTime).ToList();
                cachedTime = DateTime.Now;
            }
            return cachedIssues;
        }
        public virtual async Task<IEnumerable<Issue>> GetSubtasks(Issue issue)
        {
            return (await issue.GetSubTasksAsync()).AsEnumerable();
        }
        public virtual async Task<T> ExecuteRequestAsync<T>(Method method, string resource, object requestBody = null, CancellationToken token = default(CancellationToken))
        {
            return await _jiraInstance.RestClient.ExecuteRequestAsync<T>(method, resource, requestBody, token);
        }
        public virtual async Task<IEnumerable<Worklog>> GetWorklogsAsync(Issue issue, CancellationToken token = default(CancellationToken))
        {
            return await issue.GetWorklogsAsync();
        }
        public virtual Task<Issue> GetIssueAsync(string issueKey, CancellationToken token = default(CancellationToken))
        {
            return _jiraInstance.Issues.GetIssueAsync(issueKey, token);
        }

        public virtual async Task<IEnumerable<Issue>> GetIssuesFromJqlAsync(string jql, int? maxIssues = null, int startAt = 0, CancellationToken token = default(CancellationToken))
        {
            return await _jiraInstance.Issues.GetIssuesFromJqlAsync(jql, maxIssues, startAt, token);
        }
        public virtual async Task<IEnumerable<IssueChangeLog>> GetChangeLogsAsync(Issue issue, CancellationToken token = default(CancellationToken))
        {
            return await issue.GetChangeLogsAsync(token);
        }
        public virtual Task<IEnumerable<Project>> GetProjectsAsync(CancellationToken token = default(CancellationToken))
        {
            return _jiraInstance.Projects.GetProjectsAsync(token);
        }
    }
}
