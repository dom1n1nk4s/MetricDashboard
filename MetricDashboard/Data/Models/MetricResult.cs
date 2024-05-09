using MetricDashboard.Data.Enums;

namespace MetricDashboard.Data.Models
{
    public class MetricResult
    {
        public int Id { get; set; }
        public MetricEnum MetricEnum { get; set; }
        public double Score { get; set; }
        public string ObjectsAffectingScore { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public TimeScopeEnum TimeScope { get; set; }
    }
}
