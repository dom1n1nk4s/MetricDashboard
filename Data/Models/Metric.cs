using MetricDashboard.Data.Enums;

namespace MetricDashboard.Data.Models
{
    public class Metric
    {
        public MetricEnum MetricEnum { get; set; }
        public bool IsDisabled { get; set; }
        public string Settings { get; set; } = string.Empty;
        public MetricSystemEnum System { get; set; }
        public RadialSettings RadialSettings { get; set; }
    }
}
