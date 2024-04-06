using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class FailedRecoveryCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.FAILED_DEPLOYMENT_RECOVERY_TIME;

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