using MetricDashboard.Data.Enums;

namespace MetricDashboard.Data.Models
{
    public class ColorRange
    {
        public int Id { get; set; }
        public ColorEnum Color { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int RadialSettingsId { get; set; }
    }
}
