using MetricDashboard.Data;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class ChangeFailureCalculator : IMetricCalculator
    {
        public void Calculate(ApplicationDbContext dbcontext, Atlassian.Jira.Jira jira)
        {
            throw new NotImplementedException();
        }
    }
}