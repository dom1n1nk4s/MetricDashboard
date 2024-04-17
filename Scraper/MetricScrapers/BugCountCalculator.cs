using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class BugCountCalculator : IMetricCalculator
    {
        /*
         * select average bug count per last 10 sprints.
         * options: scope (check last X sprints)
         */
        public MetricEnum MetricEnum => MetricEnum.BUG_COUNT;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}