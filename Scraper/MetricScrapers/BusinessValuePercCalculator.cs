using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class BusinessValuePercCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.BUSINESS_VALUE_PERCENTAGE;

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