using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class TaskHandoverCalculator : IMetricCalculator
    {
        /*
         * foreach task
         * get all subtasks where words are not "Analysis, Testing, Test,"
         * results.add(select unique assignees count)
         * result average out
         * 
         * 
         * options: scope (for the previous 6 months, month, week, sprint);
         */
        public MetricEnum MetricEnum => MetricEnum.TASK_HANDOVERS_BEFORE_COMPLETION;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}