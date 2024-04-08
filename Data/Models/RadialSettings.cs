using MetricDashboard.Data.Enums;

namespace MetricDashboard.Data.Models
{
    public class RadialSettings
    {
        public int Id { get; set; }
        public MetricEnum MetricEnum { get; set; }
        public double Value { get; set; }
        public int GreenRange { get; set; }
        public int YellowRange { get; set; }
        public int RedRange { get; set; }
        public int Length { get; set; } = 260;

        public (string color, int range)[] GetRanges()
        {
            return new (string color, int range)[]
            {
                ("green", GreenRange),
                ("yellow", YellowRange),
                ("red", RedRange)
            }.OrderBy(x => x.range).ToArray();
        }
    }
}
