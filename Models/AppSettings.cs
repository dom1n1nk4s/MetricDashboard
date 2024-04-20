namespace MetricDashboard.Models
{
    public class AppSettings
    {
        public string JiraUrl { get; set; }
        public string JiraUsername { get; set; }
        public string JiraPassword { get; set; }
        public string BitbucketConsumerKey { get; set; }
        public string BitbucketConsumerSecretKey { get; set; }
    }
}