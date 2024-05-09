using MetricDashboard.Models;
using MetricDashboard.Services;
using Microsoft.Extensions.Options;
using Moq;
using SharpBucket.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricDashboard.Tests.Mocks
{
    internal static class MocksFactory
    {
        public static Mock<JiraService> GetJiraService()
        {
            var optionsMock = new Mock<IOptions<AppSettings>>();
            optionsMock.Setup(o => o.Value).Returns(new AppSettings
            {
                JiraUrl = "https://example.com",
                JiraUsername = "username",
                JiraPassword = "password"
            });

            return new Mock<JiraService>(optionsMock.Object);
        }
        public static Mock<BitBucketService> GetBitbucketService()
        {
            var bitbucket = new SharpBucketV2();
            return new Mock<BitBucketService>(bitbucket);
        }
    }
}
