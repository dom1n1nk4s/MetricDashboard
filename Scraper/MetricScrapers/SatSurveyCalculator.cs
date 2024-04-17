using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class SatSurveyCalculator : IMetricCalculator
    {
        /*
         * options: manual entry button for client satisfaction of core scrum survey numbers (get the ones that were used in workplace )
         * average all inputted stuff and store as average with Date.
         */
        public MetricEnum MetricEnum => MetricEnum.SATISFACTION_SURVEY;

        public async Task Calculate()
        {
            return;
        }
    }
}