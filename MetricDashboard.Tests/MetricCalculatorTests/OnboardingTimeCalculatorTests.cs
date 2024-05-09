using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Issue = Atlassian.Jira.Issue;

namespace MetricDashboard.Tests.MetricCalculatorTests
{
    public class OnboardingTimeCalculatorTests
    {
        [Fact]
        public async Task Calculate_Should_Add_MetricResult_To_Database()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<OnboardingTimeCalculator>>();

            var dbFactory = new TestDbContextFactory();

            var jiraServiceMock = MocksFactory.GetJiraService();
            var jiraMock = new Mock<Jira>(new ServiceLocator(), null);

            var jiraIssue = new Issue(jiraMock.Object, new RemoteIssue()
            {
                key = "TEST-1",
                id = "TEST-1",
                summary = "ONBOARDING",
                created = DateTime.Now,
                type = new RemoteIssueType { name = "Task" },
            }, null);

            var worklog1 = new Worklog(null, default);
            var authorUser1 = new JiraUser(new RemoteJiraUser { emailAddress = "user1@example.com" });
            worklog1.SetPrivate(x => x.AuthorUser, authorUser1);
            worklog1.SetPrivate(x => x.TimeSpentInSeconds, 8 * 60 * 60 * 24);

            var worklog2 = new Worklog(null, default);
            var authorUser2 = new JiraUser(new RemoteJiraUser { emailAddress = "user2@example.com" });
            worklog2.SetPrivate(x => x.AuthorUser, authorUser2);
            worklog2.SetPrivate(x => x.TimeSpentInSeconds, 6 * 60 * 60 * 24);

            var worklog3 = new Worklog(null, default);
            var authorUser3 = new JiraUser(new RemoteJiraUser { emailAddress = "user3@example.com" });
            worklog3.SetPrivate(x => x.AuthorUser, authorUser3);
            worklog3.SetPrivate(x => x.TimeSpentInSeconds, 4 * 60 * 60 * 24);

            using (var context = dbFactory.CreateDbContext())
            {
                context.Metrics.Add(new Metric
                {
                    MetricEnum = MetricEnum.ONBOARDING_TIME,
                    Settings = $"{{\"OnboardingTaskId\":\"{jiraIssue.JiraIdentifier}\"}}"
                });
                context.SaveChanges();
            }
            jiraServiceMock.Setup(s => s.GetInstance()).Returns(jiraMock.Object);
            jiraServiceMock.Setup(s => s.GetIssueAsync(jiraIssue.JiraIdentifier,default)).Returns(Task.FromResult(jiraIssue));
            jiraServiceMock.Setup(s => s.GetWorklogsAsync(jiraIssue,default)).Returns(Task.FromResult(new List<Worklog> { worklog1,worklog2,worklog3}.AsEnumerable()));

            var calculator = new OnboardingTimeCalculator(loggerMock.Object, jiraServiceMock.Object, dbFactory);

            // Act
            await calculator.Calculate();

            // Assert
            using (var context = dbFactory.CreateDbContext())
            {
                var metricEnum = MetricEnum.ONBOARDING_TIME;
                var metricResults = await context.MetricResults.Where(x => x.MetricEnum == metricEnum).ToListAsync();
                Assert.Single(metricResults);
                Assert.NotNull(metricResults.First());
                Assert.Equal(6, metricResults.First().Score);
            }
        }
    }
}
