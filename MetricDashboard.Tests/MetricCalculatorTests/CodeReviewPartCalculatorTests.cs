using SharpBucket.V2.EndPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricDashboard.Tests.MetricCalculatorTests
{
    public class CodeReviewPartCalculatorTests
    {
        //pr where 2 users inv and afk
        // pr where 2 users inv but one approved
        // pr where users inv but one approved and other commented
        // avg 50%
        [Fact]
        public async Task Calculate_Should_Add_MetricResult_To_Database()
        {

            // Arrange
            var loggerMock = new Mock<ILogger<CodeReviewPartCalculator>>();
            var bitbucketServiceMock = MocksFactory.GetBitbucketService();

            var user = new User()
            {
                display_name= "owner",
                account_id = "user1"
            };
            var repo = new Repository()
            {
                name = "some-repo"
            };
            bitbucketServiceMock.Setup(x => x.GetCache(false)).Returns((user,new List<Repository> { repo}));

            var user2 = new User()
            {
                display_name = "user2",
                account_id = "user2"
            };
            var user3 = new User()
            {
                display_name = "user3",
                account_id = "user3"
            };

            var pullRequest1 = new PullRequest()
            {
                id = 1,
                title = "pr1",
                reviewers = new List<User> { user2,user3 }
            };
            var pullRequest2 = new PullRequest()
            {
                id = 2,
                title = "pr1",
                reviewers = new List<User> { user2, user3 }
            };
            var pullRequest3 = new PullRequest()
            {
                id = 3,
                title = "pr1",
                reviewers = new List<User> { user2, user3 }
            };
            var activities1 = new List<Activity>()
            {
            };
            var activities2 = new List<Activity>()
            {
                new Activity()
                {
                    approval = new Approval
                    {
                        user = user2
                    }
                }
            };
            var activities3 = new List<Activity>()
            {
                new Activity()
                {
                    approval = new Approval
                    {
                        user = user2
                    }
                },
                new Activity
                {
                    comment = new BComment()
                    {
                        user = new UserInfo()
                        {
                            account_id = user3.account_id,
                            display_name = user3.display_name,
                        }
                    }
                }
            };
            bitbucketServiceMock.Setup(x=>x.ListPullRequestActivities(user.display_name, repo.name, pullRequest1.id.Value)).Returns(activities1);
            bitbucketServiceMock.Setup(x=>x.ListPullRequestActivities(user.display_name, repo.name, pullRequest2.id.Value)).Returns(activities2);
            bitbucketServiceMock.Setup(x=>x.ListPullRequestActivities(user.display_name, repo.name, pullRequest3.id.Value)).Returns(activities3);
            bitbucketServiceMock.Setup(x => x.ListPullRequests(It.IsAny<string>(), repo.name, It.IsAny<ListPullRequestsParameters>()))
                .Returns(new List<PullRequest> { pullRequest1, pullRequest2, pullRequest3 });
            bitbucketServiceMock.Setup(x => x.GetPullRequestAsync(user.display_name, repo.name, pullRequest1.id.Value, default)).Returns(Task.FromResult(pullRequest1));
            bitbucketServiceMock.Setup(x => x.GetPullRequestAsync(user.display_name, repo.name, pullRequest2.id.Value, default)).Returns(Task.FromResult(pullRequest2));
            bitbucketServiceMock.Setup(x => x.GetPullRequestAsync(user.display_name, repo.name, pullRequest3.id.Value, default)).Returns(Task.FromResult(pullRequest3));


            var dbFactory = new TestDbContextFactory();

            var calculator = new CodeReviewPartCalculator(dbFactory, loggerMock.Object, bitbucketServiceMock.Object);

            // Act
            await calculator.Calculate();

            // Assert
            using (var context = dbFactory.CreateDbContext())
            {
                var metricEnum = MetricEnum.CODE_REVIEW_PARTICIPATION;
                var metricResults = await context.MetricResults.Where(x => x.MetricEnum == metricEnum).ToListAsync();
                Assert.Single(metricResults);
                Assert.NotNull(metricResults.First());
                Assert.Equal(50, metricResults.First().Score);
            }
        }
    }
}
