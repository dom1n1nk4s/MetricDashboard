using Atlassian.Jira;
using MetricDashboard.Models;
using Microsoft.Extensions.Options;

namespace MetricDashboard.Services
{
    public class JiraService
    {
        private readonly Jira _jiraInstance;
        public JiraService(IOptions<AppSettings> options)
        {
            var settings = options.Value;
            _jiraInstance = Jira.CreateRestClient(settings.JiraUrl, settings.JiraUsername, settings.JiraPassword);

        }
        public Jira GetInstance () { return _jiraInstance; }
    }
}
