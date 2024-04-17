using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class CodeIntegTimeCalculator : IMetricCalculator
    {
        /*
         * foreach task
         * var relatedDeploymentTime = (get deployment where task has been deployed).DateTime
         * results.add(relatedDeploymentTime - Task.CreationTime)
         * 
         * average results out. output as days
         * 
         * options: scope (for the previous 6 months, month, week, sprint)
         */
        public MetricEnum MetricEnum => MetricEnum.CODE_INTEGRATION_TIME;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}