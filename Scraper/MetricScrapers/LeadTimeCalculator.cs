using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class LeadTimeCalculator : IMetricCalculator
    {
        /*
         * var deployments = get all deployments for scope
         * foreach tasks.Where(x => x.status == done)
         * var taskDeploymentTime = task.RelatedPr.RelatedDeployment
         * result.add (taskDeploymentTime - task.CreationTime)
         * 
         * options: scope (for the previous 6 months, month, week, sprint);
         */
        public MetricEnum MetricEnum => MetricEnum.LEAD_TIME_FOR_CHANGES;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}