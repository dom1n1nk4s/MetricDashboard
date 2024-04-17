using MetricDashboard.Data.Enums;

namespace MetricDashboard.Extensions
{
    public static class EnumExtensions
    {
        public static DateTime GetDateTime(this TimeScopeEnum timeScope, int sprintLength = 14)
        {
            DateTime now = DateTime.Now;

            switch (timeScope)
            {
                case TimeScopeEnum.SIX_MONTHS:
                    return now.AddMonths(-6);
                case TimeScopeEnum.ONE_MONTH:
                    return now.AddMonths(-1);
                case TimeScopeEnum.TWO_WEEKS:
                    return now.AddDays(-14);
                case TimeScopeEnum.ONE_WEEK:
                    return now.AddDays(-7);
                case TimeScopeEnum.ONE_SPRINT:
                    return now.AddDays(-sprintLength);
                default:
                    return now;
            }
        }
    }
}
