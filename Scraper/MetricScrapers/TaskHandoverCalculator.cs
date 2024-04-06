using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class TaskHandoverCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.TASK_HANDOVERS_BEFORE_COMPLETION;

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