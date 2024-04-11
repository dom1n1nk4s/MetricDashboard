using MetricDashboard.Data.Enums;

namespace MetricDashboard.Data.Models
{
    public class RadialSettings
    {
        public int Id { get; set; }
        public MetricEnum MetricEnum { get; set; }
        public double Value { get; set; }
        public List<ColorRange> ColorRanges { get; set; } = new List<ColorRange>();
        public int Start { get; set; } 
        public int End { get; set; } = 260;
        public int Step { get; set; } = 26;
        public string PointerUnits { get; set; } = string.Empty;
        //TODO: insert scoring overrides here.
    }
}
