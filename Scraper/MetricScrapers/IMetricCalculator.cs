using Atlassian.Jira;
using MetricDashboard.Data;

namespace MetricDashboard.Scraper.MetricScrapers
{
    public interface IMetricCalculator
    {
        public void Calculate(ApplicationDbContext dbcontext, Jira jira);
    }
}
