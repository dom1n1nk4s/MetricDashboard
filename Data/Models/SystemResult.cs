using MetricDashboard.Data.Enums;

namespace MetricDashboard.Data.Models
{
    public class SystemResult
    {
        public int Id { get; set; }
        public MetricSystemEnum SystemEnum { get; set; }
        public TimeScopeEnum TimeScope { get; set; }
        public double Score { get; set; }
        public DateTime Date {  get; set; } = DateTime.Now;
    }
}
