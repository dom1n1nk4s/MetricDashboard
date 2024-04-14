using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class SatSurveyCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.SATISFACTION_SURVEY;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}