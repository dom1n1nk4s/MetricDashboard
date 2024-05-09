using Radzen;

namespace MetricDashboard.Extensions
{
    public static class NotificationServiceExtensions
    {
        public static void NotifySuccess(this NotificationService n, string message, string details = null, int duration = 2000)
        {
            n.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Success,
                Summary = message,
                 Detail = details,
                Duration = duration
            });
        }
        public static void NotifyError(this NotificationService n, string message, string details = null, int duration = 2000)
        {
            n.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = message,
                Detail = details,
                Duration = duration
            });
        }
    }
}
