using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class ChangeFailureCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.CHANGE_FAILURE_RATE;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}