using MetricDashboard.Data;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class FailedRecoveryCalculator : IMetricCalculator
    {
        public void Calculate(ApplicationDbContext dbcontext, Atlassian.Jira.Jira jira)
        {
            throw new NotImplementedException();
        }
    }
}