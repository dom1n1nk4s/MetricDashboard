using MetricDashboard.Data.Enums;

namespace MetricDashboard.Data.Models
{
    public class GlobalMetricSettings
    {
        public int Id { get; set; }
        public TimeScopeEnum Scope { get; set; }
        public int SprintLength { get; set; }
    }
}
