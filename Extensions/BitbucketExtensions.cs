using SharpBucket.V2;
using SharpBucket.V2.Pocos;

namespace MetricDashboard.Extensions
{
    public static class BitbucketExtensions
    {
        private static User cachedUser = new();
        private static List<Repository> cachedRepos = new List<Repository>();
        private static DateTime cachedTime = DateTime.MinValue;
        public static (User user, List<Repository> repos) GetCache(this SharpBucketV2 sharpBucket, bool forceRefresh = false)
        {
            if ((DateTime.Now - cachedTime).TotalMinutes > 60 || forceRefresh)
            {
                cachedUser = sharpBucket.UserEndPoint().GetUser();
                cachedRepos = sharpBucket.RepositoriesEndPoint().RepositoriesResource(cachedUser.display_name).ListRepositories();
                cachedTime = DateTime.Now;
            }
            return (cachedUser, cachedRepos);
        }
    }
}
