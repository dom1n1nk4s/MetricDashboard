using MetricDashboard.Data.Enums;
using MetricDashboard.Data.Models;
using MetricDashboard.Scraper.MetricScrapers;

namespace MetricDashboard.Helpers
{
    public static class MetricHelper
    {
        private static readonly Dictionary<MetricEnum, IMetricCalculator> _metricToScraperMap = new()
        {
            { MetricEnum.DEPLOYMENT_FREQUENCY, new DeployFreqCalculator() },
            { MetricEnum.LEAD_TIME_FOR_CHANGES, new LeadTimeCalculator() },
            { MetricEnum.FAILED_DEPLOYMENT_RECOVERY_TIME, new FailedRecoveryCalculator() },
            { MetricEnum.CHANGE_FAILURE_RATE, new ChangeFailureCalculator() },
            { MetricEnum.SATISFACTION_SURVEY, new SatSurveyCalculator() },
            { MetricEnum.WORKER_RETENTION_RATE, new WorkerRetRateCalculator() },
            { MetricEnum.BUG_COUNT, new BugCountCalculator() },
            { MetricEnum.CLIENT_SATISFACTION_SURVEY, new ClientSatSurveyCalculator() },
            { MetricEnum.CODE_TASK_MERGE_COMMIT_COUNT, new CodeTaskMergeCountCalculator() },
            { MetricEnum.CODE_REVIEW_PARTICIPATION, new CodeReviewPartCalculator() },
            { MetricEnum.TIME_SPENT_WORKING, new TimeSpentWorkCalculator() },
            { MetricEnum.CODE_INTEGRATION_TIME, new CodeIntegTimeCalculator() },
            { MetricEnum.ONBOARDING_TIME, new OnboardingTimeCalculator() },
            { MetricEnum.TASK_HANDOVERS_BEFORE_COMPLETION, new TaskHandoverCalculator() },
            { MetricEnum.WORKFLOW_INTERRUPTION_TIME, new WorkflowIntCalculator() },
            { MetricEnum.BUSINESS_VALUE_PERCENTAGE, new BusinessValuePercCalculator() }
        };
        public static IMetricCalculator GetMetricCalculator(MetricEnum metricEnum)
        {
            return _metricToScraperMap[metricEnum];
        }
    }
}
