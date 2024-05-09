using SharpBucket.V2.EndPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project = Atlassian.Jira.Project;

namespace MetricDashboard.Tests.MetricCalculatorTests
{
    public class CodeTaskMergeCountCalculatorTests
    {
        [Fact]
        public async Task Calculate_Should_Add_MetricResult_To_Database()
        {

            // Arrange
            var loggerMock = new Mock<ILogger<CodeTaskMergeCountCalculator>>();
            var bitbucketServiceMock = MocksFactory.GetBitbucketService();

            var user = new User()
            {
                display_name = "owner",
                account_id = "user1"
            };
            var repo = new Repository()
            {
                name = "some-repo"
            };
            var repo2 = new Repository()
            {
                name = "some-repo2222"
            };
            bitbucketServiceMock.Setup(x => x.GetCache(false)).Returns((user, new List<Repository> { repo, repo2 }));

            var pullRequest11 = new PullRequest()
            {
                id = 1,
                title = "pr1",
                source = new Source
                {
                    branch = new Branch
                    {
                        name = "feature/scrum-2"
                    },
                }
            };
            var pullRequest12 = new PullRequest()
            {
                id = 2,
                title = "pr2",
                source = new Source
                {
                    branch = new Branch
                    {
                        name = "feature/kanban-2"
                    },
                }
            };

            bitbucketServiceMock.Setup(x => x.ListPullRequests(user.display_name, repo.name, It.IsAny<ListPullRequestsParameters>()))
                .Returns(new List<PullRequest> { pullRequest11, pullRequest12 });

            var pullRequest21 = new PullRequest()
            {
                id = 1,
                title = "pr1",
                source = new Source
                {
                    branch = new Branch
                    {
                        name = "feature/scrum-2"
                    },
                }
            };

            bitbucketServiceMock.Setup(x => x.ListPullRequests(user.display_name, repo2.name, It.IsAny<ListPullRequestsParameters>()))
                .Returns(new List<PullRequest> { pullRequest21, });

            var commits = new List<Commit>()
            {
                new Commit(),
                new Commit(),
                new Commit(),
            };
            bitbucketServiceMock.Setup(x => x.ListCommits(user.display_name,repo.name,null,0)).Returns(commits);

            var commits2 = new List<Commit>()
            { 
                new Commit()
            };
            bitbucketServiceMock.Setup(x => x.ListCommits(user.display_name,repo2.name,null,0)).Returns(commits2);

            var _jiraServiceMock = MocksFactory.GetJiraService();

            var project1 = new Project(null, new RemoteProject() { name = "project1", key = "scrum"});

            _jiraServiceMock.Setup(x => x.GetProjectsAsync(default)).Returns(Task.FromResult(new List<Project> { project1 }.AsEnumerable()));

            var dbFactory = new TestDbContextFactory();

            var calculator = new CodeTaskMergeCountCalculator(loggerMock.Object, bitbucketServiceMock.Object, dbFactory, _jiraServiceMock.Object);

            // Act
            await calculator.Calculate();

            // Assert
            using (var context = dbFactory.CreateDbContext())
            {
                var metricEnum = MetricEnum.CODE_TASK_MERGE_COMMIT_COUNT;
                var metricResults = await context.MetricResults.Where(x => x.MetricEnum == metricEnum).ToListAsync();
                Assert.Single(metricResults);
                Assert.NotNull(metricResults.First());
                Assert.Equal(9, metricResults.First().Score);
            }
        }
    }
}
