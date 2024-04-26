using MetricDashboard.Data.Enums;

namespace MetricDashboard.Data.Models
{
    public class GlobalMetricSettings
    {
        public int Id { get; set; }
        public TimeScopeEnum Scope { get; set; }
        public int SprintLength { get; set; }
        public int GreenCalculationValue { get; set; }
        public int YellowCalculationValue { get; set; }
        public int RedCalculationValue { get; set; }
    }
}
