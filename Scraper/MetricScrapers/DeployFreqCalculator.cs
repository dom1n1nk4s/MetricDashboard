using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class DeployFreqCalculator : IMetricCalculator
    {
        /*
         * get total deployment count for scope
         * options: scope (for the previous 6 months, month, week, sprint)
         */
        public MetricEnum MetricEnum => MetricEnum.DEPLOYMENT_FREQUENCY;
        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}