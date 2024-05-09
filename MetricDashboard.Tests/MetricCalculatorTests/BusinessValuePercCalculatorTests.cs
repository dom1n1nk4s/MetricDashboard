using Atlassian.Jira;
using MetricDashboard.Data.Models;
using MetricDashboard.Scraper.MetricScrapers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using MetricDashboard.Tests.Mocks;
using MetricDashboard.Data.Enums;
using Atlassian.Jira.Remote;
using MetricDashboard.Tests.Helpers;
using Issue = Atlassian.Jira.Issue;

namespace MetricDashboard.Tests.MetricCalculatorTests
{
    public class BusinessValuePercCalculatorTests
    {
        [Fact]
        public async Task Calculate_Should_Add_MetricResult_To_Database()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<BusinessValuePercCalculator>>();

            var jiraServiceMock = MocksFactory.GetJiraService();
            var jiraMock = new Mock<Jira>(new ServiceLocator(), null);

            var jiraIssueMock = new Mock<Issue>(jiraMock.Object, new RemoteIssue()
            {
                key = "TEST-1",
                summary = "TEST-1",
                created = DateTime.Now,
                type = new RemoteIssueType { name = "Task" },
            }, null);
            var timeTracking1 = new IssueTimeTrackingData(null);
            var timeTracking2 = new IssueTimeTrackingData(null);
            timeTracking1.SetPrivate(x => x.TimeSpentInSeconds, 7200);
            timeTracking2.SetPrivate(x => x.TimeSpentInSeconds, 1800);
            var issues = new List<Issue>
            {
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "do smth",
                    key = "TEST-2",
                    created = DateTime.Now,
                    type = new RemoteIssueType{name = "Task", subTask = true},
                    timeTracking = timeTracking1,
                }),
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "test stuff",
                    key = "TEST-3",
                    created = DateTime.Now,
                    type = new RemoteIssueType{name = "Task", subTask = true},
                    timeTracking = timeTracking2,
                }),
            }.AsEnumerable();

            jiraServiceMock.Setup(j => j.GetSubtasks(It.IsAny<Issue>()))
                .Returns(Task.FromResult(issues));
            jiraServiceMock.Setup(s => s.GetInstance()).Returns(jiraMock.Object);
            jiraServiceMock.Setup(s => s.GetCachedIssues(It.IsAny<GlobalMetricSettings>(), It.IsAny<bool>())).Returns(new List<Issue> { jiraIssueMock.Object });

            var dbFactory = new TestDbContextFactory();

            var calculator = new BusinessValuePercCalculator(loggerMock.Object, jiraServiceMock.Object, dbFactory);

            // Act
            await calculator.Calculate();

            // Assert
            using (var context = dbFactory.CreateDbContext())
            {
                var metricEnum = MetricEnum.BUSINESS_VALUE_PERCENTAGE;
                var metricResults = await context.MetricResults.Where(x => x.MetricEnum == metricEnum).ToListAsync();
                Assert.Single(metricResults);
                Assert.NotNull(metricResults.First());
                Assert.Equal(80d, metricResults.First().Score, 2);
            }
        }
    }
}
