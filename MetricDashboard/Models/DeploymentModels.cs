namespace MetricDashboard.Models
{
    using System;
    using System.Collections.Generic;

    public class Deployable
    {
        public string Type { get; set; }
        public string Uuid { get; set; }
        public Pipeline Pipeline { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Commit Commit { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class Pipeline
    {
        public string Uuid { get; set; }
        public string Type { get; set; }
    }

    public class Commit
    {
        public string Hash { get; set; }
        public Links Links { get; set; }
        public string Type { get; set; }
    }

    public class Links
    {
        public Self Self { get; set; }
        public Html Html { get; set; }
    }

    public class Self
    {
        public string Href { get; set; }
    }

    public class Html
    {
        public string Href { get; set; }
    }

    public class Environment
    {
        public string Uuid { get; set; }
    }

    public class Release
    {
        public string Type { get; set; }
        public string Uuid { get; set; }
        public Pipeline Pipeline { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Commit Commit { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class State
    {
        public string Type { get; set; }
        public string Url { get; set; }
        public Status Status { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime CompletedOn { get; set; }
        public string Name { get; set; }
    }

    public class Status
    {
        public string Type { get; set; }
        public string Name { get; set; }
    }

    public class Deployment
    {
        public Deployable Deployable { get; set; }
        public string Type { get; set; }
        public int Number { get; set; }
        public string Uuid { get; set; }
        public string Key { get; set; }
        public Step Step { get; set; }
        public Environment Environment { get; set; }
        public Release Release { get; set; }
        public DateTime CreatedOn { get; set; }
        public State State { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int Version { get; set; }
    }

    public class Step
    {
        public string Uuid { get; set; }
    }

    public class DeploymentResponse
    {
        public int Page { get; set; }
        public List<Deployment> Values { get; set; }
        public int Size { get; set; }
        public int Pagelen { get; set; }
    }

}
