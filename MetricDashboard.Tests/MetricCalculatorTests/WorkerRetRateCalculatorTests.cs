using MetricDashboard.Models;
using Moq;
using SharpBucket.V2.Pocos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Issue = Atlassian.Jira.Issue;

namespace MetricDashboard.Tests.MetricCalculatorTests
{
    public class WorkerRetRateCalculatorTests
    {
        [Fact]
        public async Task Calculate_Should_Add_MetricResult_To_Database()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<WorkerRetRateCalculator>>();
            var jiraServiceMock = MocksFactory.GetJiraService();

            var jiraMock = new Mock<Jira>(new ServiceLocator(), null);

            var user1 = new JiraUser(new RemoteJiraUser
            {
                displayName = "user111",
                accountId = "user111",
                locale = "en-US",
            });
            var user2 = new JiraUser(new RemoteJiraUser
            {
                displayName = "user222",
                accountId = "user222",
                locale = "en-US",
            });
            var user3 = new JiraUser(new RemoteJiraUser
            {
                displayName = "user333",
                accountId = "user333",
                locale = null
            });
            var user4 = new JiraUser(new RemoteJiraUser
            {
                displayName = "user444",
                accountId = "user444",
                locale = "en-UK"
            });

            var issues1 = new List<Issue>
            {
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "do smth",
                    id = "TEST-1",
                    key = "TEST-1",
                    created = DateTime.Parse("2024-01-01", CultureInfo.InvariantCulture),
                    type = new RemoteIssueType { name = "Task", subTask = false },
                    status = new RemoteStatus {name ="Done" },
                }),
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "smth do",
                    id = "TEST-2",
                    key = "TEST-2",
                    created = DateTime.Parse("2020-01-01", CultureInfo.InvariantCulture),
                    type = new RemoteIssueType { name = "Task", subTask = false },
                    status = new RemoteStatus {name ="Done" },
                }),                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "smth do",
                    id = "TEST-3",
                    key = "TEST-3",
                    created = DateTime.Parse("2022-01-01", CultureInfo.InvariantCulture),
                    type = new RemoteIssueType { name = "Task", subTask = false },
                    status = new RemoteStatus {name ="Done" },
                }),
            };
            var issues2 = new List<Issue>
            {
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "do smth",
                    id = "TEST-4",
                    key = "TEST-4",
                    created = DateTime.Parse("2024-01-01", CultureInfo.InvariantCulture),
                    type = new RemoteIssueType { name = "Task", subTask = false },
                    status = new RemoteStatus {name ="Done" },
                }),
            };
            List<Issue> issues3 = null;
            var issues4 = new List<Issue>
            {
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "do smth",
                    id = "TEST-5",
                    key = "TEST-5",
                    created = DateTime.Parse("2024-01-01", CultureInfo.InvariantCulture),
                    type = new RemoteIssueType { name = "Task", subTask = false },
                    status = new RemoteStatus {name ="Done" },
                }),
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "do smth",
                    id = "TEST-6",
                    key = "TEST-6",
                    created = DateTime.Parse("2022-01-01", CultureInfo.InvariantCulture),
                    type = new RemoteIssueType { name = "Task", subTask = false },
                    status = new RemoteStatus {name ="Done" },
                }),
            };
            jiraServiceMock.Setup(s => s.GetIssuesFromJqlAsync($"reporter = \"{user1.AccountId}\" OR assignee = \"{user1.AccountId}\"", 10000, 0, default))
                .Returns(Task.FromResult(issues1.AsEnumerable()));
            jiraServiceMock.Setup(s => s.GetIssuesFromJqlAsync($"reporter = \"{user2.AccountId}\" OR assignee = \"{user2.AccountId}\"", 10000, 0, default))
                .Returns(Task.FromResult(issues2.AsEnumerable()));
            jiraServiceMock.Setup(s => s.GetIssuesFromJqlAsync($"reporter = \"{user3.AccountId}\" OR assignee = \"{user3.AccountId}\"", 10000, 0, default))
                .Returns(Task.FromResult(issues3?.AsEnumerable()));
            jiraServiceMock.Setup(s => s.GetIssuesFromJqlAsync($"reporter = \"{user4.AccountId}\" OR assignee = \"{user4.AccountId}\"", 10000, 0, default))
                .Returns(Task.FromResult(issues4.AsEnumerable()));
            jiraServiceMock.Setup(s => s.ExecuteRequestAsync<List<JiraUser>>(
            It.IsAny<RestSharp.Method>(), $"/rest/api/3/users/search", null!, default))
                .Returns(Task.FromResult(new List<JiraUser> { user1, user2, user3, user4 }));


            var dbFactory = new TestDbContextFactory();

            var calculator = new WorkerRetRateCalculator(loggerMock.Object, jiraServiceMock.Object, dbFactory);

            // Act
            await calculator.Calculate();

            // Assert
            using (var context = dbFactory.CreateDbContext())
            {
                var metricEnum = MetricEnum.WORKER_RETENTION_RATE;
                var metricResults = await context.MetricResults.Where(x => x.MetricEnum == metricEnum).ToListAsync();
                Assert.Single(metricResults);
                Assert.NotNull(metricResults.First());
                Assert.Equal(24, metricResults.First().Score);
            }
        }
    }
}
