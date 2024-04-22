namespace MetricDashboard.Models
{

    public class Author
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
    }

    public class Source
    {
        public string Branch { get; set; }
        public string Url { get; set; }
    }

    public class Destination
    {
        public string Branch { get; set; }
        public string Url { get; set; }
    }

    public class PullRequest
    {
        public Author Author { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int CommentCount { get; set; }
        public Source Source { get; set; }
        public Destination Destination { get; set; }
        public List<object> Reviewers { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        public DateTime LastUpdate { get; set; }
        public string RepositoryId { get; set; }
        public string RepositoryName { get; set; }
        public string RepositoryUrl { get; set; }
        public string RepositoryAvatarUrl { get; set; }
    }

    public class Detail
    {
        public List<PullRequest> PullRequests { get; set; }
    }

    public class PullRequestResponse
    {
        public List<Detail> Detail { get; set; } //always contains one
        /*
         var response = await _jira.RestClient.ExecuteRequestAsync<PullRequestResponse>(RestSharp.Method.GET,
                    $"/rest/dev-status/1.0/issue/detail?issueId={issue.JiraIdentifier}&applicationType=bitbucket&dataType=pullrequest");
         */
    }

}
