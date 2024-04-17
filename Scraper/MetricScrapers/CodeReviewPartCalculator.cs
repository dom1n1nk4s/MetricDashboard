using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    internal class CodeReviewPartCalculator : IMetricCalculator
    {
        /*
         * 
         * foreach PR
         * var reviewers = (get reviewers assigned to pr)
         * result.add( reviewers.Count(x => x.APPROVED_OR_LEFT_COMMENT) / reviewers.Count())
         * 
         * 
         * options: scope (for the previous 6 months, month, week, sprint)
         */
        public MetricEnum MetricEnum => MetricEnum.CODE_REVIEW_PARTICIPATION;

        public async Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}