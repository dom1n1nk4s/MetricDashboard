using System.ComponentModel.DataAnnotations;

namespace MetricDashboard.Data.Enums
{
    public enum TimeScopeEnum
    {
        NONE = 0,
        [Display(Description = "Six months")]
        SIX_MONTHS,
        [Display(Description = "One month")]
        ONE_MONTH,
        [Display(Description = "Two weeks")]
        TWO_WEEKS,
        [Display(Description = "One week")]
        ONE_WEEK,
        [Display(Description = "Custom scope")]
        CUSTOM_SCOPE,
    }
}
