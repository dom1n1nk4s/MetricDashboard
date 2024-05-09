using MetricDashboard.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Issue = Atlassian.Jira.Issue;

namespace MetricDashboard.Tests.MetricCalculatorTests
{
    public class LeadTimeCalculatorTests
    {
        [Fact]
        public async Task Calculate_Should_Add_MetricResult_To_Database()
        {

            // Arrange
            var loggerMock = new Mock<ILogger<LeadTimeCalculator>>();
            var bitbucketServiceMock = MocksFactory.GetBitbucketService();
            var scopeDateTime = TimeScopeEnum.SIX_MONTHS.GetDateTime();
            var user = new User()
            {
                display_name = "owner",
                account_id = "user1"
            };
            var repo = new Repository()
            {
                name = "some-repo",
                slug = "some-repo"
            };
            var repo2 = new Repository()
            {
                name = "some-other-repo",
                slug = "some-other-repo",
            };
            bitbucketServiceMock.Setup(x => x.GetCache(false)).Returns((user, new List<Repository> { repo, repo2 }));

            var workspace = new Workspace()
            {
                slug = "some-workspace"
            };
            bitbucketServiceMock.Setup(x => x.GetWorkspace()).Returns(workspace);

            var environment11 = new DeploymentEnvironment
            {
                name = "production",
                uuid = "1",
            };
            var environment12 = new DeploymentEnvironment
            {
                name = "test",
                uuid = "2",
            };
            var environment21 = new DeploymentEnvironment
            {
                name = "production",
                uuid = "3",
            };
            var environment22 = new DeploymentEnvironment
            {
                name = "test",
                uuid = "4",
            };

            bitbucketServiceMock.Setup(x => x.ListEnvironmentsAsync(user.display_name, repo.name)).Returns(Task.FromResult(new List<DeploymentEnvironment> { environment11, environment12 }));
            bitbucketServiceMock.Setup(x => x.ListEnvironmentsAsync(user.display_name, repo2.name)).Returns(Task.FromResult(new List<DeploymentEnvironment> { environment21, environment22 }));

            var deploymentResponse = new DeploymentResponse
            {
                Values = new List<Deployment>
                {
                    new Deployment
                    {
                        State = new State
                        {
                            Name = "COMPLETED",
                            CompletedOn = DateTime.Parse("2024-01-01", CultureInfo.InvariantCulture),
                        },
                        Environment = new Models.Environment
                        {
                            Uuid = environment11.uuid
                        }
                    },
                    new Deployment
                    {
                        State = new State
                        {
                            Name = "COMPLETED",
                            CompletedOn = DateTime.Parse("2024-01-03", CultureInfo.InvariantCulture),
                        },
                        Environment = new Models.Environment
                        {
                            Uuid = environment11.uuid
                        }
                    },
                    new Deployment
                    {
                        State = new State
                        {
                            Name = "COMPLETED",
                            CompletedOn = DateTime.Parse("2024-01-06", CultureInfo.InvariantCulture),
                        },
                        Environment = new Models.Environment
                        {
                            Uuid = environment11.uuid
                        }
                    },
                    new Deployment
                    {
                        State = new State
                        {
                            Name = "SOMEOTHERSTATE",
                            CompletedOn = DateTime.Parse("2024-01-01", CultureInfo.InvariantCulture),
                        },
                        Environment = new Models.Environment
                        {
                            Uuid = environment11.uuid
                        }
                    },
                    new Deployment
                    {
                        State = new State
                        {
                            Name = "COMPLETED",
                            CompletedOn = DateTime.Parse("2024-01-01", CultureInfo.InvariantCulture),
                        },
                        Environment = new Models.Environment
                        {
                            Uuid = environment12.uuid
                        }
                    },
                }
            };

            bitbucketServiceMock.Setup(x => x.GetAsync<DeploymentResponse>(
                $"/repositories/{workspace.slug}/{repo.slug}/deployments?q=self.state.completed_on > {scopeDateTime.ToString("yyyy-MM-ddTHH:mm:00zzz")}"
                , It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(deploymentResponse));

            var deploymentResponse2 = new DeploymentResponse
            {
                Values = new List<Deployment>
                {
                    new Deployment
                    {
                        State = new State
                        {
                            Name = "COMPLETED",
                            CompletedOn = DateTime.Parse("2024-01-01", CultureInfo.InvariantCulture),
                        },
                        Environment = new Models.Environment
                        {
                            Uuid = environment21.uuid
                        }
                    },
                    new Deployment
                    {
                        State = new State
                        {
                            Name = "COMPLETED",
                            CompletedOn = DateTime.Parse("2024-01-05", CultureInfo.InvariantCulture),
                        },
                        Environment = new Models.Environment
                        {
                            Uuid = environment21.uuid
                        }
                    },
                    new Deployment
                    {
                        State = new State
                        {
                            Name = "COMPLETED",
                            CompletedOn = DateTime.Parse("2024-01-10", CultureInfo.InvariantCulture),
                        },
                        Environment = new Models.Environment
                        {
                            Uuid = environment21.uuid
                        }
                    },
                    new Deployment
                    {
                        State = new State
                        {
                            Name = "SOMEOTHERSTATE",
                            CompletedOn = DateTime.Parse("2024-01-01", CultureInfo.InvariantCulture),
                        },
                        Environment = new Models.Environment
                        {
                            Uuid = environment21.uuid
                        }
                    },
                    new Deployment
                    {
                        State = new State
                        {
                            Name = "COMPLETED",
                            CompletedOn = DateTime.Parse("2024-01-01", CultureInfo.InvariantCulture),
                        },
                        Environment = new Models.Environment
                        {
                            Uuid = environment22.uuid
                        }
                    },
                }
            };

            bitbucketServiceMock.Setup(x => x.GetAsync<DeploymentResponse>(
                $"/repositories/{workspace.slug}/{repo2.slug}/deployments?q=self.state.completed_on > {scopeDateTime.ToString("yyyy-MM-ddTHH:mm:00zzz")}"
                , It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(deploymentResponse2));


            var jiraServiceMock = MocksFactory.GetJiraService();
            var jiraMock = new Mock<Jira>(new ServiceLocator(), null);

            var issue0 = new Issue(jiraMock.Object, new RemoteIssue()
            {
                key = "TEST-0",
                id = "TEST-0",
                summary = "TEST-0",
                created = DateTime.Parse("2024-01-02", CultureInfo.InvariantCulture),
                type = new RemoteIssueType { name = "critical" },
                status = new RemoteStatus { name = "done" }
            }, null);
            var issue1 = new Issue(jiraMock.Object, new RemoteIssue()
            {
                key = "TEST-1",
                id = "TEST-1",
                summary = "TEST-1",
                created = DateTime.Parse("2024-01-01", CultureInfo.InvariantCulture),
                type = new RemoteIssueType { name = "task" },
                status = new RemoteStatus { name = "done" }
            }, null);
            var issue2 = new Issue(jiraMock.Object, new RemoteIssue()
            {
                key = "TEST-2",
                id = "TEST-2",
                summary = "TEST-2",
                created = DateTime.Parse("2024-01-03", CultureInfo.InvariantCulture),
                type = new RemoteIssueType { name = "bug" },
                status = new RemoteStatus { name = "done" }
            }, null);
            var issue3 = new Issue(jiraMock.Object, new RemoteIssue()
            {
                key = "TEST-3",
                id = "TEST-3",
                summary = "TEST-3",
                created = DateTime.MaxValue,
                type = new RemoteIssueType { name = "Task" },
                status = new RemoteStatus { name = "in progress" }
            }, null);
            var issue4 = new Issue(jiraMock.Object, new RemoteIssue()
            {
                key = "TEST-4",
                id = "TEST-4",
                summary = "TEST-4",
                created = DateTime.MaxValue,
                type = new RemoteIssueType { name = "critical" },
                status = new RemoteStatus { name = "in progress" }
            }, null);
            jiraServiceMock.Setup(x => x.GetCachedIssues(It.IsAny<GlobalMetricSettings>(), false)).Returns(new List<Issue> { issue0, issue1, issue2, issue3, issue4 });

            var pullRequestResponse0 = new PullRequestResponse
            {
                Detail = new List<Detail>
                {
                    new Detail
                    {
                        PullRequests = new List<Models.PullRequest>
                        {
                            new Models.PullRequest
                            {
                                Status = "merged",
                                LastUpdate = DateTime.Parse("2024-01-04", CultureInfo.InvariantCulture),
                                RepositoryName = repo.slug,
                            }
                        }
                    }
                }
            };
            jiraServiceMock.Setup(x => x.ExecuteRequestAsync<PullRequestResponse>(RestSharp.Method.GET,
                 $"/rest/dev-status/1.0/issue/detail?issueId={issue0.JiraIdentifier}&applicationType=bitbucket&dataType=pullrequest", null, default))
                .Returns(Task.FromResult(pullRequestResponse0));

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
                                Status = "merged",
                                LastUpdate = DateTime.Parse("2024-01-02", CultureInfo.InvariantCulture),
                                RepositoryName = repo.slug,
                            }
                        }
                    }
                }
            };
            jiraServiceMock.Setup(x => x.ExecuteRequestAsync<PullRequestResponse>(RestSharp.Method.GET,
                 $"/rest/dev-status/1.0/issue/detail?issueId={issue1.JiraIdentifier}&applicationType=bitbucket&dataType=pullrequest", null, default))
                .Returns(Task.FromResult(pullRequestResponse1));

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
                                Status = "merged",
                                LastUpdate = DateTime.Parse("2024-01-06", CultureInfo.InvariantCulture),
                                RepositoryName = repo2.slug,
                            }
                        }
                    }
                }
            };
            jiraServiceMock.Setup(x => x.ExecuteRequestAsync<PullRequestResponse>(RestSharp.Method.GET,
                 $"/rest/dev-status/1.0/issue/detail?issueId={issue2.JiraIdentifier}&applicationType=bitbucket&dataType=pullrequest", null, default))
                .Returns(Task.FromResult(pullRequestResponse2));

            var dbFactory = new TestDbContextFactory();

            var calculator = new LeadTimeCalculator(loggerMock.Object, bitbucketServiceMock.Object, dbFactory, jiraServiceMock.Object);

            // Act
            await calculator.Calculate();

            // Assert
            using (var context = dbFactory.CreateDbContext())
            {
                var metricEnum = MetricEnum.LEAD_TIME_FOR_CHANGES;
                var metricResults = await context.MetricResults.Where(x => x.MetricEnum == metricEnum).ToListAsync();
                Assert.Single(metricResults);
                Assert.NotNull(metricResults.First());
                Assert.Equal(4.33d, metricResults.First().Score, 2);
            }
        }
    }
}
