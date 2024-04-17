using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class TimeSpentWorkCalculator : IMetricCalculator
    {
        /*
         * two ways of doing this
         * 
         * 1. if a user allocates time on subtask AND THEN the time reflects on the task
         * foreach task
         * get total sum of time spent on task
         * average out over all tasks
         * 
         * 2. if a user allocates time on subtask AND THEN the time DOES NOT reflect on the task
         * foreach task
         * get total sum of all subtask time spent
         * average out over all tasks
         * 
         * options: scope (for the previous 6 months, month, week, sprint);
         */
        public MetricEnum MetricEnum => MetricEnum.TIME_SPENT_WORKING;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}