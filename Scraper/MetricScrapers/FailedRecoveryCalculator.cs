using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class FailedRecoveryCalculator : IMetricCalculator
    {
        /*
         * var critIncidentDeployments = Tasks.where(type is critical incident).select(inc => inc.RelatedPR.RelatedDeployment);
         * foreach critIncidentDeployments
         * var fixDeployment = find next deployment of same project
         * result.add( fixDeployment.DateTime - critIncidentDeployments.DateTime)
         * result average out
         * 
         * options: scope (for the previous 6 months, month, week, sprint); globalopt: critical incident type (optional)
         */
        public MetricEnum MetricEnum => MetricEnum.FAILED_DEPLOYMENT_RECOVERY_TIME;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}