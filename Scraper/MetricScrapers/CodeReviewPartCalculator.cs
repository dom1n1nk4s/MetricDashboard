using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class CodeReviewPartCalculator : IMetricCalculator
    {
        public MetricEnum MetricEnum => MetricEnum.CODE_REVIEW_PARTICIPATION;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}