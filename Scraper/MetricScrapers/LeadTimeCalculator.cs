using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class LeadTimeCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.LEAD_TIME_FOR_CHANGES;

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