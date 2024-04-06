using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class DeployFreqCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.DEPLOYMENT_FREQUENCY;
        public void Calculate()
        {
            throw new NotImplementedException();
        }
    }
}