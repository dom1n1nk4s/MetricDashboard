using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class CodeIntegTimeCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.CODE_INTEGRATION_TIME;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}