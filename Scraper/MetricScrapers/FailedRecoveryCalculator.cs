using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class FailedRecoveryCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.FAILED_DEPLOYMENT_RECOVERY_TIME;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}