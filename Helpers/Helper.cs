using MetricDashboard.Data.Enums;
using MetricDashboard.Data.Models;
using MetricDashboard.Extensions;
using Microsoft.Identity.Client;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MetricDashboard.Helpers
{
    public static class Helper
    {
        public static List<(int from, int to)> GetOverCoveredRanges(IEnumerable<ColorRange> ranges)
        {
            var coveredPoints = new HashSet<int>();
            var overCoveredPoints = new List<(int from, int to)>();

            foreach (var range in ranges)
            {
                for (int i = range.From; i < range.To; i++)
                {
                    if (!coveredPoints.Add(i))
                    {
                        if (overCoveredPoints.Count == 0 || overCoveredPoints[^1].to != i)
                        {
                            overCoveredPoints.Add((i, i + 1));
                        }
                        else
                        {
                            overCoveredPoints[^1] = (overCoveredPoints[^1].from, i + 1);
                        }
                    }
                }
            }

            return overCoveredPoints;
        }
        public static List<(int from, int to)> GetNotCoveredRanges(IEnumerable<ColorRange> ranges, int start, int length)
        {
            // Initialize a boolean array to mark covered positions
            bool[] covered = new bool[length];

            // Mark covered positions based on the ranges provided
            foreach (var range in ranges)
            {
                for (int i = range.From; i < range.To && i < length; i++)
                {
                    covered[i] = true;
                }
            }

            // Initialize a list to store the not covered ranges
            var notCoveredRanges = new List<(int from, int to)>();

            // Iterate through the covered positions to find gaps
            int gapStart = -1;
            for (int i = start; i < length; i++)
            {
                if (!covered[i])
                {
                    if (gapStart == -1)
                    {
                        gapStart = i;
                    }
                }
                else
                {
                    if (gapStart != -1)
                    {
                        notCoveredRanges.Add((gapStart, i));
                        gapStart = -1;
                    }
                }
            }

            // If a gap is found at the end, add it to the list of not covered ranges
            if (gapStart != -1)
            {
                notCoveredRanges.Add((gapStart, length));
            }

            return notCoveredRanges;
        }
       
        public static string[] GetObjectsAffectingScore(MetricEnum metric, string serializedObjects)
        {
            switch (metric)
            {
                case MetricEnum.DEPLOYMENT_FREQUENCY:
                    var deployments = serializedObjects.Deserialize<List<(string repoKey, double deploymentCount)>>();
                    return deployments.Select(x => $"Repository \"{x.repoKey}\" has had {x.deploymentCount} deployments").ToArray();
                case MetricEnum.LEAD_TIME_FOR_CHANGES:
                    var leadTimes = serializedObjects.Deserialize<List<(string issueKey, double days)>>();
                    return leadTimes.Select(x => $"Issue \"{x.issueKey}\" has a lead time of {x.days:N} days").ToArray();
                case MetricEnum.FAILED_DEPLOYMENT_RECOVERY_TIME:
                    var recoveryTimes = serializedObjects.Deserialize<List<(string repoKey, double hoursToFix)>>();
                    return recoveryTimes.Select(x => $"Repository \"{x.repoKey}\" took {x.hoursToFix:N} hours to recover from failed deployment").ToArray();
                case MetricEnum.CHANGE_FAILURE_RATE:
                    var failureRates = serializedObjects.Deserialize<List<(string repoKey, int incidentCausingDeploymentCount, int totalDeploymentCount)>>();
                    return failureRates.Select(x => $"Repository \"{x.repoKey}\" has a change failure rate of {(double)x.incidentCausingDeploymentCount / x.totalDeploymentCount:P2}").ToArray();
                case MetricEnum.SATISFACTION_SURVEY:
                    return new string[] { serializedObjects };
                case MetricEnum.WORKER_RETENTION_RATE:
                    var workerRetention = serializedObjects.Deserialize<List<(string Name, DateTime ActiveFrom, DateTime ActiveTo)>>();
                    return workerRetention.Select(x => $"\"{x.Name}\" was active from {x.ActiveFrom.ToShortDateString()} to {x.ActiveTo.ToShortDateString()}").ToArray();
                case MetricEnum.BUG_COUNT:
                    var bugs = serializedObjects.Deserialize<List<(string name, DateTime dateTime)>>();
                    return bugs.Select(x => $"Bug \"{x.name}\" was reported on {x.dateTime.ToShortDateString()}").ToArray();
                case MetricEnum.CLIENT_SATISFACTION_SURVEY:
                    return new string[] { serializedObjects };
                case MetricEnum.CODE_TASK_MERGE_COMMIT_COUNT:
                    var commitCounts = serializedObjects.Deserialize<List<(string repoName, int taskCount, int pullRequestCount, int commitCount)>>();
                    return commitCounts.Select(x => $"Repository \"{x.repoName}\" has {x.taskCount} tasks, {x.pullRequestCount} pull requests, and {x.commitCount} commits").ToArray();
                case MetricEnum.CODE_REVIEW_PARTICIPATION:
                    var reviewParticipation = serializedObjects.Deserialize<List<(string pullRequestName, double participationPercent)>>();
                    return reviewParticipation.Select(x => $"Pull request \"{x.pullRequestName}\" has a participation rate of {x.participationPercent:P2}").ToArray();
                case MetricEnum.TIME_SPENT_WORKING:
                    var timeSpentWorking = serializedObjects.Deserialize<List<(string issueKey, double countOfHoursPerTask)>>();
                    return timeSpentWorking.Select(x => $"Issue \"{x.issueKey}\" took {x.countOfHoursPerTask:N} hours to complete").ToArray();
                case MetricEnum.CODE_INTEGRATION_TIME:
                    var integrationTimes = serializedObjects.Deserialize<List<(string issueKey, double hours)>>();
                    return integrationTimes.Select(x => $"Issue \"{x.issueKey}\" took {x.hours:N} hours to integrate").ToArray();
                case MetricEnum.ONBOARDING_TIME:
                    var onboardingTimes = serializedObjects.Deserialize<List<(string, long)>>();
                    return onboardingTimes.Select(x => $"Onboarding for \"{x.Item1}\" took {x.Item2:N} days").ToArray();
                case MetricEnum.TASK_HANDOVERS_BEFORE_COMPLETION:
                    var handovers = serializedObjects.Deserialize<List<(string issueKey, int countOfPeopleWorking)>>();
                    return handovers.Select(x => $"Issue \"{x.issueKey}\" had {x.countOfPeopleWorking} people working on it before completion").ToArray();
                case MetricEnum.WORKFLOW_INTERRUPTION_TIME:
                    var interruptionTimes = serializedObjects.Deserialize<List<(string issueKey, double durationOnHoldInHours)>>();
                    return interruptionTimes.Select(x => $"Issue \"{x.issueKey}\" was on hold for {x.durationOnHoldInHours:N} hours").ToArray();
                case MetricEnum.BUSINESS_VALUE_PERCENTAGE:
                    var businessValues = serializedObjects.Deserialize<List<(string issueKey, double countOfHoursWorking, double countOfTotalHours)>>();
                    return businessValues.Select(x => $"Issue \"{x.issueKey}\" has a business value percentage of {(x.countOfHoursWorking / x.countOfTotalHours) * 100:P2}").ToArray();
                default:
                    return Array.Empty<string>();
            }
        }
    }
}
