using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class TimeSpentWorkCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.TIME_SPENT_WORKING;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}