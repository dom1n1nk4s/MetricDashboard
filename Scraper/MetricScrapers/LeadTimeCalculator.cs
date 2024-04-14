using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class LeadTimeCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.LEAD_TIME_FOR_CHANGES;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}