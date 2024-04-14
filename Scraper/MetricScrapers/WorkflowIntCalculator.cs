using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class WorkflowIntCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.WORKFLOW_INTERRUPTION_TIME;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}