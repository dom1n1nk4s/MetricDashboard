using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Extensions;
using MetricDashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class OnboardingTimeCalculator : IMetricCalculator
    {
        public void Calculate(ApplicationDbContext dbcontext, Jira jira)
        {
            var settings = dbcontext.Metrics.AsNoTracking().First(x => x.MetricEnum == Data.Enums.MetricEnum.ONBOARDING_TIME)?.Settings?.Deserialize<OnboardingTimeSettings>();
            if(settings == null)
            {
                //log...
                throw new ArgumentNullException(nameof(settings));
            }
            var taskId = settings.OnboardingTaskId;
            var issue = jira.Issues.GetIssueAsync(taskId).Result;
            var eachPersonsTimeSpent = issue.GetWorklogsAsync().Result.GroupBy(x => x.AuthorUser.AccountId).Select(x => (x.Key, x.Select(z => z.TimeSpentInSeconds).Sum()));
            var average = eachPersonsTimeSpent.Select(x => x.Item2).Average();

            //save metricCalculation values as work logs 
        }
    }
}