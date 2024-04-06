using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class WorkflowIntCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.WORKFLOW_INTERRUPTION_TIME;

        public void Calculate(ApplicationDbContext dbcontext, Atlassian.Jira.Jira jira)
        {
            throw new NotImplementedException();
        }

        public void Calculate()
        {
            throw new NotImplementedException();
        }
    }
}