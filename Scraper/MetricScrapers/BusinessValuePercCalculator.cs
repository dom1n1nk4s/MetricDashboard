using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class BusinessValuePercCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.BUSINESS_VALUE_PERCENTAGE;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}