using MetricDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricDashboard.Tests.MetricCalculatorTests
{
    public class DeployFreqCalculatorTests
    {
        [Fact]
        public async Task Calculate_Should_Add_MetricResult_To_Database()
        {

            // Arrange
            var loggerMock = new Mock<ILogger<DeployFreqCalculator>>();
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

            var dbFactory = new TestDbContextFactory();

            var calculator = new DeployFreqCalculator(bitbucketServiceMock.Object, loggerMock.Object, dbFactory);

            // Act
            await calculator.Calculate();

            // Assert
            using (var context = dbFactory.CreateDbContext())
            {
                var metricEnum = MetricEnum.DEPLOYMENT_FREQUENCY;
                var metricResults = await context.MetricResults.Where(x => x.MetricEnum == metricEnum).ToListAsync();
                Assert.Single(metricResults);
                Assert.NotNull(metricResults.First());
                Assert.Equal(2, metricResults.First().Score);
            }
        }
    }
}
