using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class CodeTaskMergeCountCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.CODE_TASK_MERGE_COMMIT_COUNT;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}