using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class BusinessValuePercCalculator : IMetricCalculator
    {
        /*
         * foreach task: filter out each substask containing words "PR, Code review, Analysis, etc.", then get all timesheet allocated hours on the subtasks which are left.
         * calculate the sum(LeftoverTasks) and divide from total time spent on this task (perhaps a sum of all subtasks including filtered).
         * then average through all of tasks.
         * 
         * options: scope (for the previous 6 months, month, week, sprint)
         */
        public MetricEnum MetricEnum => MetricEnum.BUSINESS_VALUE_PERCENTAGE;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}