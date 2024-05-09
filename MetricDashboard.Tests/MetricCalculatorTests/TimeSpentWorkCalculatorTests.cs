using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Issue = Atlassian.Jira.Issue;

namespace MetricDashboard.Tests.MetricCalculatorTests
{
    public class TimeSpentWorkCalculatorTests
    {
        [Fact]
        public async Task Calculate_Should_Add_MetricResult_To_Database()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<TimeSpentWorkCalculator>>();

            var jiraServiceMock = MocksFactory.GetJiraService();
            var jiraMock = new Mock<Jira>(new ServiceLocator(), null);

            var issue1 = new Issue(jiraMock.Object, new RemoteIssue()
            {
                key = "TEST-1",
                id = "TEST-1",
                summary = "TEST-1",
                created = DateTime.Now,
                type = new RemoteIssueType { name = "Task" },
            }, null);
            var issue2 = new Issue(jiraMock.Object, new RemoteIssue()
            {
                key = "TEST-2",
                id = "TEST-2",
                summary = "TEST-1",
                created = DateTime.Now,
                type = new RemoteIssueType { name = "Task" },
            }, null);

            var timeTracking1 = new IssueTimeTrackingData(null);
            var timeTracking2 = new IssueTimeTrackingData(null);
            timeTracking1.SetPrivate(x => x.TimeSpentInSeconds, 4 * 60 * 60);
            timeTracking2.SetPrivate(x => x.TimeSpentInSeconds, 6 * 60 * 60);
            var subTasks1 = new List<Issue>
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
            var timeTracking3 = new IssueTimeTrackingData(null);
            var timeTracking4 = new IssueTimeTrackingData(null);
            var timeTracking5 = new IssueTimeTrackingData(null);
            timeTracking3.SetPrivate(x => x.TimeSpentInSeconds, 18 * 60 * 60);
            timeTracking4.SetPrivate(x => x.TimeSpentInSeconds, 12 * 60 * 60);
            timeTracking5.SetPrivate(x => x.TimeSpentInSeconds, 2 * 60 * 60);
            var subTasks2 = new List<Issue>
            {
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "do smth",
                    key = "TEST-4",
                    created = DateTime.Now,
                    type = new RemoteIssueType{name = "Task", subTask = true},
                    timeTracking = timeTracking3,
                }),
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "test stuff",
                    key = "TEST-5",
                    created = DateTime.Now,
                    type = new RemoteIssueType{name = "Task", subTask = true},
                    timeTracking = timeTracking4,
                }),
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "do stuff %^2",
                    key = "TEST-6",
                    created = DateTime.Now,
                    type = new RemoteIssueType{name = "Task", subTask = true},
                    timeTracking = timeTracking5,
                }),
            }.AsEnumerable();

            jiraServiceMock.Setup(s => s.GetSubtasks(issue1)).Returns(Task.FromResult(subTasks1));
            jiraServiceMock.Setup(s => s.GetSubtasks(issue2)).Returns(Task.FromResult(subTasks2));
            jiraServiceMock.Setup(s => s.GetInstance()).Returns(jiraMock.Object);
            jiraServiceMock.Setup(s => s.GetCachedIssues(It.IsAny<GlobalMetricSettings>(), It.IsAny<bool>())).Returns(new List<Issue> { issue1, issue2 });

            var dbFactory = new TestDbContextFactory();

            var calculator = new TimeSpentWorkCalculator(loggerMock.Object, jiraServiceMock.Object, dbFactory);

            // Act
            await calculator.Calculate();

            // Assert
            using (var context = dbFactory.CreateDbContext())
            {
                var metricEnum = MetricEnum.TIME_SPENT_WORKING;
                var metricResults = await context.MetricResults.Where(x => x.MetricEnum == metricEnum).ToListAsync();
                Assert.Single(metricResults);
                Assert.NotNull(metricResults.First());
                Assert.Equal(21d, metricResults.First().Score, 2);
            }
        }
    }
}
