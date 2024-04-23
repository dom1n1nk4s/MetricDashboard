using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class ChangeFailureCalculator : IMetricCalculator
    {
        /*
         * var critIncidentDeployments = Tasks.where(type is critical incident).select(inc => inc.RelatedPR.RelatedDeployment);
         * get critical incident causing deployments COUNT  / (get all deploymentsCOUNT - get all hotfix related deploymentsCOUNT)
         * options: scope (for the previous 6 months, month, week, sprint); globalopt: critical incident type (optional)
         */
        public MetricEnum MetricEnum => MetricEnum.CHANGE_FAILURE_RATE;

        public async Task Calculate()
        {
            try {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.ToString());
            }
        }
    }
}