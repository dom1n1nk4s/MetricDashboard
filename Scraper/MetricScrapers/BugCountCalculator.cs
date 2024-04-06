using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class BugCountCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.BUG_COUNT;

        public void Calculate(ApplicationDbContext dbcontext, Atlassian.Jira.Jira jira)
        {
            throw new NotImplementedException();
        }

        public void Calculate()
        {
            throw new NotImplementedException();
        }
    }
}