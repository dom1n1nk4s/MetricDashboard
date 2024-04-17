using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class CodeTaskMergeCountCalculator : IMetricCalculator
    {
        /*
         * sum of all code task, merge, commit counts.
         * options: scope (for the previous 6 months, month, week, sprint)
         */
        public MetricEnum MetricEnum => MetricEnum.CODE_TASK_MERGE_COMMIT_COUNT;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}