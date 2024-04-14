using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class TaskHandoverCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.TASK_HANDOVERS_BEFORE_COMPLETION;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}