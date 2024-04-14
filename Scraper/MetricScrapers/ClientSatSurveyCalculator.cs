using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class ClientSatSurveyCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.CLIENT_SATISFACTION_SURVEY;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}