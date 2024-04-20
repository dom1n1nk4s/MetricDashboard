using Atlassian.Jira;
using MetricDashboard.Models;
using Microsoft.Extensions.Options;
using SharpBucket.V2;

namespace MetricDashboard.Services
{
    public class BitBucketService
    {
        private readonly SharpBucketV2 _bitbucketInstance;
        public BitBucketService(IOptions<AppSettings> options)
        {
            var settings = options.Value;
            _bitbucketInstance = new SharpBucketV2();
            _bitbucketInstance.OAuth2ClientCredentials(settings.BitbucketConsumerKey, settings.BitbucketConsumerSecretKey);
        }
        public SharpBucketV2 GetInstance() { return _bitbucketInstance; }
    }
}
