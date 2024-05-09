using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Issue = Atlassian.Jira.Issue;

namespace MetricDashboard.Tests.MetricCalculatorTests
{
    public class WorkflowIntCalculatorTests
    {
        //issue1 where todo - in progress - done
        //issue2 where todo - in progress - on hold - in progress - done
        // issue3 where todo - in progress - on hold - in progress - on hold - done

        [Fact]
        public async Task Calculate_Should_Add_MetricResult_To_Database()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<WorkflowIntCalculator>>();

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
            var issue3 = new Issue(jiraMock.Object, new RemoteIssue()
            {
                key = "TEST-3",
                id = "TEST-3",
                summary = "TEST-3",
                created = DateTime.Now,
                type = new RemoteIssueType { name = "Task" },
            }, null);
            //---
            var changeLogItem11 = new IssueChangeLogItem();
            changeLogItem11.SetPrivate(x => x.FieldName, "status");
            changeLogItem11.SetPrivate(x => x.FromValue, "todo");
            changeLogItem11.SetPrivate(x => x.ToValue, "in progress");
            var changeLog11 = new IssueChangeLog();
            changeLog11.SetPrivate(x => x.Items, new List<IssueChangeLogItem> { changeLogItem11 });
            changeLog11.SetPrivate(x => x.CreatedDate, DateTime.Parse("2024-01-01", CultureInfo.InvariantCulture));

            var changeLogItem12 = new IssueChangeLogItem();
            changeLogItem12.SetPrivate(x => x.FieldName, "status");
            changeLogItem12.SetPrivate(x => x.FromValue, "in progress");
            changeLogItem12.SetPrivate(x => x.ToValue, "done");
            var changeLog12 = new IssueChangeLog();
            changeLog12.SetPrivate(x => x.Items, new List<IssueChangeLogItem> { changeLogItem12 });
            changeLog12.SetPrivate(x => x.CreatedDate, DateTime.Parse("2024-01-02", CultureInfo.InvariantCulture));

            jiraServiceMock.Setup(s => s.GetChangeLogsAsync(issue1,default)).Returns(Task.FromResult(new List<IssueChangeLog> { changeLog11, changeLog12}.AsEnumerable()));
            //---
            var changeLogItem21 = new IssueChangeLogItem();
            changeLogItem21.SetPrivate(x => x.FieldName, "status");
            changeLogItem21.SetPrivate(x => x.FromValue, "todo");
            changeLogItem21.SetPrivate(x => x.ToValue, "in progress");
            var changeLog21 = new IssueChangeLog();
            changeLog21.SetPrivate(x => x.Items, new List<IssueChangeLogItem> { changeLogItem21 });
            changeLog21.SetPrivate(x => x.CreatedDate, DateTime.Parse("2024-01-01", CultureInfo.InvariantCulture));

            var changeLogItem22 = new IssueChangeLogItem();
            changeLogItem22.SetPrivate(x => x.FieldName, "status");
            changeLogItem22.SetPrivate(x => x.FromValue, "in progress");
            changeLogItem22.SetPrivate(x => x.ToValue, "on hold");
            var changeLog22 = new IssueChangeLog();
            changeLog22.SetPrivate(x => x.Items, new List<IssueChangeLogItem> { changeLogItem22 });
            changeLog22.SetPrivate(x => x.CreatedDate, DateTime.Parse("2024-01-02", CultureInfo.InvariantCulture));

            var changeLogItem23 = new IssueChangeLogItem();
            changeLogItem23.SetPrivate(x => x.FieldName, "status");
            changeLogItem23.SetPrivate(x => x.FromValue, "on hold");
            changeLogItem23.SetPrivate(x => x.ToValue, "in progress");
            var changeLog23 = new IssueChangeLog();
            changeLog23.SetPrivate(x => x.Items, new List<IssueChangeLogItem> { changeLogItem23 });
            changeLog23.SetPrivate(x => x.CreatedDate, DateTime.Parse("2024-01-03", CultureInfo.InvariantCulture));

            var changeLogItem24 = new IssueChangeLogItem();
            changeLogItem24.SetPrivate(x => x.FieldName, "status");
            changeLogItem24.SetPrivate(x => x.FromValue, "in progress");
            changeLogItem24.SetPrivate(x => x.ToValue, "done");
            var changeLog24 = new IssueChangeLog();
            changeLog24.SetPrivate(x => x.Items, new List<IssueChangeLogItem> { changeLogItem24 });
            changeLog24.SetPrivate(x => x.CreatedDate, DateTime.Parse("2024-01-04", CultureInfo.InvariantCulture));

            jiraServiceMock.Setup(s => s.GetChangeLogsAsync(issue2, default)).Returns(Task.FromResult(new List<IssueChangeLog> { changeLog21, changeLog22, changeLog23, changeLog24 }.AsEnumerable()));
            //---
            var changeLogItem31 = new IssueChangeLogItem();
            changeLogItem31.SetPrivate(x => x.FieldName, "status");
            changeLogItem31.SetPrivate(x => x.FromValue, "todo");
            changeLogItem31.SetPrivate(x => x.ToValue, "in progress");
            var changeLog31 = new IssueChangeLog();
            changeLog31.SetPrivate(x => x.Items, new List<IssueChangeLogItem> { changeLogItem31 });
            changeLog31.SetPrivate(x => x.CreatedDate, DateTime.Parse("2024-01-01", CultureInfo.InvariantCulture));

            var changeLogItem32 = new IssueChangeLogItem();
            changeLogItem32.SetPrivate(x => x.FieldName, "status");
            changeLogItem32.SetPrivate(x => x.FromValue, "in progress");
            changeLogItem32.SetPrivate(x => x.ToValue, "on hold");
            var changeLog32 = new IssueChangeLog();
            changeLog32.SetPrivate(x => x.Items, new List<IssueChangeLogItem> { changeLogItem32 });
            changeLog32.SetPrivate(x => x.CreatedDate, DateTime.Parse("2024-01-02", CultureInfo.InvariantCulture));

            var changeLogItem33 = new IssueChangeLogItem();
            changeLogItem33.SetPrivate(x => x.FieldName, "status");
            changeLogItem33.SetPrivate(x => x.FromValue, "on hold");
            changeLogItem33.SetPrivate(x => x.ToValue, "in progress");
            var changeLog33 = new IssueChangeLog();
            changeLog33.SetPrivate(x => x.Items, new List<IssueChangeLogItem> { changeLogItem33 });
            changeLog33.SetPrivate(x => x.CreatedDate, DateTime.Parse("2024-01-03", CultureInfo.InvariantCulture));

            var changeLogItem34 = new IssueChangeLogItem();
            changeLogItem34.SetPrivate(x => x.FieldName, "status");
            changeLogItem34.SetPrivate(x => x.FromValue, "in progress");
            changeLogItem34.SetPrivate(x => x.ToValue, "on hold");
            var changeLog34 = new IssueChangeLog();
            changeLog34.SetPrivate(x => x.Items, new List<IssueChangeLogItem> { changeLogItem34 });
            changeLog34.SetPrivate(x => x.CreatedDate, DateTime.Parse("2024-01-04", CultureInfo.InvariantCulture));

            var changeLogItem35 = new IssueChangeLogItem();
            changeLogItem35.SetPrivate(x => x.FieldName, "status");
            changeLogItem35.SetPrivate(x => x.FromValue, "on hold");
            changeLogItem35.SetPrivate(x => x.ToValue, "done");
            var changeLog35 = new IssueChangeLog();
            changeLog35.SetPrivate(x => x.Items, new List<IssueChangeLogItem> { changeLogItem35 });
            changeLog35.SetPrivate(x => x.CreatedDate, DateTime.Parse("2024-01-06", CultureInfo.InvariantCulture));

            jiraServiceMock.Setup(s => s.GetChangeLogsAsync(issue3, default)).Returns(Task.FromResult(
                new List<IssueChangeLog> { changeLog31, changeLog32, changeLog33, changeLog34, changeLog35 }.AsEnumerable()));
            //---

            jiraServiceMock.Setup(s => s.GetInstance()).Returns(jiraMock.Object);
            jiraServiceMock.Setup(s => s.GetCachedIssues(It.IsAny<GlobalMetricSettings>(), It.IsAny<bool>())).Returns(new List<Issue> { issue1, issue2, issue3 });

            var dbFactory = new TestDbContextFactory();

            var calculator = new WorkflowIntCalculator(loggerMock.Object, jiraServiceMock.Object, dbFactory);

            // Act
            await calculator.Calculate();

            // Assert
            using (var context = dbFactory.CreateDbContext())
            {
                var metricEnum = MetricEnum.WORKFLOW_INTERRUPTION_TIME;
                var metricResults = await context.MetricResults.Where(x => x.MetricEnum == metricEnum).ToListAsync();
                Assert.Single(metricResults);
                Assert.NotNull(metricResults.First());
                Assert.Equal(32d, metricResults.First().Score, 2);
            }
        }
        }
}
