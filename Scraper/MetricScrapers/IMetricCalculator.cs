using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;

namespace MetricDashboard.Scraper.MetricScrapers
{
    public interface IMetricCalculator
    {
        public MetricEnum MetricEnum { get; }
        public void Calculate();
    }
}
