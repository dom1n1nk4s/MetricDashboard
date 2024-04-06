using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class ClientSatSurveyCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.CLIENT_SATISFACTION_SURVEY;

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