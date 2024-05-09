using Atlassian.Jira;
using MetricDashboard.Models;
using Microsoft.Extensions.Options;
using SharpBucket;
using SharpBucket.V2;
using SharpBucket.V2.EndPoints;
using SharpBucket.V2.Pocos;

namespace MetricDashboard.Services
{
    public class BitBucketService
    {
        private readonly SharpBucketV2 _bitbucket;
        private User cachedUser = new();
        private List<Repository> cachedRepos = new List<Repository>();
        private DateTime cachedTime = DateTime.MinValue;
        public BitBucketService(IOptions<AppSettings> options)
        {
            var settings = options.Value;
            _bitbucket = new SharpBucketV2();
            _bitbucket.OAuth2ClientCredentials(settings.BitbucketConsumerKey, settings.BitbucketConsumerSecretKey);
        }
        public BitBucketService(SharpBucketV2 bitbucket)
        {
            _bitbucket = bitbucket;
        }
        public SharpBucketV2 GetInstance() { return _bitbucket; }
        public virtual (User user, List<Repository> repos) GetCache(bool forceRefresh = false)
        {
            if ((DateTime.Now - cachedTime).TotalMinutes > 60 || forceRefresh)
            {
                cachedUser = _bitbucket.UserEndPoint().GetUser();
                cachedRepos = _bitbucket.RepositoriesEndPoint().RepositoriesResource(cachedUser.display_name).ListRepositories();
                cachedTime = DateTime.Now;
            }
            return (cachedUser, cachedRepos);
        }

        public virtual List<SharpBucket.V2.Pocos.PullRequest> ListPullRequests(string user, string repo, ListPullRequestsParameters parameters)
        {
            return _bitbucket.RepositoriesEndPoint().RepositoryResource(user, repo).PullRequestsResource().ListPullRequests(parameters);
        }
        public virtual Task<SharpBucket.V2.Pocos.PullRequest> GetPullRequestAsync(string user, string repo, int pullRequestId, CancellationToken token = default(CancellationToken))
        {
            return _bitbucket.RepositoriesEndPoint().RepositoryResource(user, repo).PullRequestsResource().PullRequestResource(pullRequestId).GetPullRequestAsync();
        }
        public virtual List<Activity> ListPullRequestActivities(string user, string repo, int pullRequestId)
        {
            return _bitbucket.RepositoriesEndPoint().RepositoryResource(user, repo).PullRequestsResource().PullRequestResource(pullRequestId).ListPullRequestActivities();
        }
        public virtual Workspace GetWorkspace()
        {
            return _bitbucket.WorkspacesEndPoint().ListWorkspaces().First();
        }
        public virtual async Task<List<DeploymentEnvironment>> ListEnvironmentsAsync(string user, string repo)
        {
            return await _bitbucket.RepositoriesEndPoint().RepositoryResource(user, repo).EnvironmentsResource.ListEnvironmentsAsync();
        }
        public virtual async Task<T> GetAsync<T>(string relativeUrl, CancellationToken token) where T : new()
        {
            return _bitbucket.Get<T>(relativeUrl, token);
        }
        public virtual List<SharpBucket.V2.Pocos.Commit> ListCommits(string user, string repo, string branchOrTag = null, int max = 0)
        {
            return _bitbucket.RepositoriesEndPoint().RepositoryResource(user, repo).ListCommits(branchOrTag, max);
        }
    }
}
