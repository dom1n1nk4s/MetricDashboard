using MetricDashboard.Data.Models;
using MetricDashboard.Data;
using MetricDashboard.Models;
using MetricDashboard.Scraper.MetricScrapers;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Issue = Atlassian.Jira.Issue;
using System.Globalization;

namespace MetricDashboard.Tests.MetricCalculatorTests
{
    public class CodeIntegTimeCalculatorTests
    {
        [Fact]
        public async Task Calculate_Should_Add_MetricResult_To_Database()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CodeIntegTimeCalculator>>();
            var jiraServiceMock = MocksFactory.GetJiraService();

            var jiraMock = new Mock<Jira>(new ServiceLocator(), null);
            var issues = new List<Issue>
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
                    created = DateTime.Parse("2024-02-01", CultureInfo.InvariantCulture),
                    type = new RemoteIssueType { name = "Task", subTask = false },
                    status = new RemoteStatus {name ="Done" },
                }),
            };
            var pullRequestResponse1 = new PullRequestResponse
            {
                Detail = new List<Detail>
                {
                    new Detail
                    {
                        PullRequests = new List<Models.PullRequest>
                        {
                            new Models.PullRequest
                            {
                                Status = "Merged",
                                LastUpdate = DateTime.Parse("2024-01-03", CultureInfo.InvariantCulture),
                            }
                        }
                    }
                }
            };
            var pullRequestResponse2 = new PullRequestResponse
            {
                Detail = new List<Detail>
                {
                    new Detail
                    {
                        PullRequests = new List<Models.PullRequest>
                        {
                            new Models.PullRequest
                            {
                                Status = "Merged",
                                LastUpdate = DateTime.Parse("2024-02-05", CultureInfo.InvariantCulture),
                            }
                        }
                    }
                }
            };

            jiraServiceMock.Setup(s => s.GetCachedIssues(It.IsAny<GlobalMetricSettings>(), It.IsAny<bool>())).Returns(issues);
            jiraServiceMock.Setup(s => s.ExecuteRequestAsync<PullRequestResponse>(
                It.IsAny<RestSharp.Method>(), $"/rest/dev-status/1.0/issue/detail?issueId={issues.ElementAt(0).JiraIdentifier}&applicationType=bitbucket&dataType=pullrequest", null!, default))
                .ReturnsAsync(pullRequestResponse1);
            jiraServiceMock.Setup(s => s.ExecuteRequestAsync<PullRequestResponse>(
                It.IsAny<RestSharp.Method>(), $"/rest/dev-status/1.0/issue/detail?issueId={issues.ElementAt(1).JiraIdentifier}&applicationType=bitbucket&dataType=pullrequest", null!, default))
                .ReturnsAsync(pullRequestResponse2);

            var dbFactory = new TestDbContextFactory();

            var calculator = new CodeIntegTimeCalculator(jiraServiceMock.Object, dbFactory, loggerMock.Object);

            // Act
            await calculator.Calculate();

            // Assert
            using (var context = dbFactory.CreateDbContext())
            {
                var metricEnum = MetricEnum.CODE_INTEGRATION_TIME;
                var metricResults = await context.MetricResults.Where(x => x.MetricEnum == metricEnum).ToListAsync();
                Assert.Single(metricResults);
                Assert.NotNull(metricResults.First());
                Assert.Equal(72, metricResults.First().Score);
            }
        }

    }
}
