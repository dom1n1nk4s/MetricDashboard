using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class BugCountCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.BUG_COUNT;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}