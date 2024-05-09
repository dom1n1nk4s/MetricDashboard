using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Data.Enums;
using MetricDashboard.Data.Models;
using MetricDashboard.Scraper.MetricScrapers;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using MetricDashboard.Extensions;
using SharpBucket.V2.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetricDashboard.Tests.Mocks;
using Microsoft.Extensions.Options;
using MetricDashboard.Models;
using Atlassian.Jira.Remote;

namespace MetricDashboard.Tests.MetricCalculatorTests
{
    public class BugCountCalculatorTests
    {
        [Fact]
        public async Task Calculate_Should_Add_MetricResult_To_Context()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<BugCountCalculator>>();

            var jiraServiceMock = MocksFactory.GetJiraService();
            var jiraMock = new Mock<Jira>();

            var jira = Jira.CreateRestClient("https://localhost");
            var issues = new List<Atlassian.Jira.Issue>
            {
                new(jira, new RemoteIssue()
                {
                    type = new RemoteIssueType{name = "Bug"},
                    created = DateTime.Now,
                    summary = "Bug 1",
                }),
                new(jira, new RemoteIssue()
                {
                    type = new RemoteIssueType{name = "Bug"},
                    created = DateTime.Now,
                    summary = "Bug 2",
                }),
                new(jira, new RemoteIssue()
                {
                    type = new RemoteIssueType{name = "Task"},
                    created = DateTime.Now,
                    summary = "Task 1",
                }),
            };

            jiraServiceMock.Setup(s => s.GetInstance()).Returns(jira);
            jiraServiceMock.Setup(s => s.GetCachedIssues(It.IsAny<GlobalMetricSettings>(), It.IsAny<bool>())).Returns(issues);
            var dbFactory = new TestDbContextFactory();

            var calculator = new BugCountCalculator(loggerMock.Object, jiraServiceMock.Object, dbFactory);

            // Act
            await calculator.Calculate();
            
            // Assert
            using (var context = dbFactory.CreateDbContext())
            {
                var metricEnum = MetricEnum.BUG_COUNT;
                var metricResults = await context.MetricResults.Where(x => x.MetricEnum == metricEnum).ToListAsync();
                Assert.Single(metricResults);
                Assert.NotNull(metricResults.First());
                Assert.Equal(2d, metricResults.First().Score);
            }
        }
    }
}
