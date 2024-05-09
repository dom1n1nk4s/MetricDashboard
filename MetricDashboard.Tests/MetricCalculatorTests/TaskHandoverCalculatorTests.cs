using Atlassian.Jira;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Issue = Atlassian.Jira.Issue;

namespace MetricDashboard.Tests.MetricCalculatorTests
{
    public class TaskHandoverCalculatorTests
    {
        [Fact]
        public async Task Calculate_Should_Add_MetricResult_To_Context()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<TaskHandoverCalculator>>();

            var jiraServiceMock = MocksFactory.GetJiraService();
            var jiraMock = new Mock<Jira>(new ServiceLocator(), null);

            var issue1 = new Issue(jiraMock.Object, new RemoteIssue()
            {
                summary = "do smth",
                key = "TEST-0",
                id = "TEST-0",
                created = DateTime.Now,
                type = new RemoteIssueType { name = "Task", subTask = false },
            });
            var issue2 = new Issue(jiraMock.Object, new RemoteIssue()
            {
                summary = "test stuff",
                key = "TEST-1",
                id = "TEST-1",
                created = DateTime.Now,
                type = new RemoteIssueType { name = "Task", subTask = false },
            });
            var subTasks1 = new List<Issue>
            {
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "do smth",
                    key = "TEST-2",
                    assignee = "user1",
                    created = DateTime.Now,
                    type = new RemoteIssueType{name = "Task", subTask = true},
                }),
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "test stuff",
                    assignee = "user2",
                    key = "TEST-3",
                    created = DateTime.Now,
                    type = new RemoteIssueType{name = "Task", subTask = true},
                }),
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "do smth^2",
                    key = "TEST-4",
                    assignee = "user3",
                    created = DateTime.Now,
                    type = new RemoteIssueType{name = "Task", subTask = true},
                }),
            }.AsEnumerable();
            var subTasks2 = new List<Issue>
            {
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "do smth",
                    key = "TEST-5",
                    assignee = "user1",
                    created = DateTime.Now,
                    type = new RemoteIssueType{name = "Task", subTask = true},
                }),
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "test stuff",
                    assignee = "user2",
                    key = "TEST-6",
                    created = DateTime.Now,
                    type = new RemoteIssueType{name = "Task", subTask = true},
                }),
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "do smth^2",
                    key = "TEST-7",
                    assignee = "user3",
                    created = DateTime.Now,
                    type = new RemoteIssueType{name = "Task", subTask = true},
                }),
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "do smth^3",
                    key = "TEST-8",
                    assignee = "user3",
                    created = DateTime.Now,
                    type = new RemoteIssueType{name = "Task", subTask = true},
                }),
                new(jiraMock.Object, new RemoteIssue()
                {
                    summary = "do smth^3",
                    key = "TEST-8",
                    assignee = "user4",
                    created = DateTime.Now,
                    type = new RemoteIssueType{name = "Task", subTask = true},
                }),
            }.AsEnumerable();

            jiraServiceMock.Setup(s => s.GetInstance()).Returns(jiraMock.Object);
            jiraServiceMock.Setup(s => s.GetSubtasks(issue1)).Returns(Task.FromResult(subTasks1));
            jiraServiceMock.Setup(s => s.GetSubtasks(issue2)).Returns(Task.FromResult(subTasks2));
            jiraServiceMock.Setup(s => s.GetCachedIssues(It.IsAny<GlobalMetricSettings>(), It.IsAny<bool>())).Returns(new List<Issue> { issue1, issue2});
            var dbFactory = new TestDbContextFactory();

            var calculator = new TaskHandoverCalculator(loggerMock.Object, jiraServiceMock.Object, dbFactory);

            // Act
            await calculator.Calculate();

            // Assert
            using (var context = dbFactory.CreateDbContext())
            {
                var metricEnum = MetricEnum.TASK_HANDOVERS_BEFORE_COMPLETION;
                var metricResults = await context.MetricResults.Where(x => x.MetricEnum == metricEnum).ToListAsync();
                Assert.Single(metricResults);
                Assert.NotNull(metricResults.First());
                Assert.Equal(1.5d, metricResults.First().Score,2);
            }
        }
    }
}
