using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Helpers;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MetricDashboard.Scraper
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Jira _jira;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory, JiraService jiraService)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _jira = jiraService.GetInstance();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    CalculateMetrics();
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(60 * 60 * 1000, stoppingToken); // 60 minutes
            }
            
        }
        private void CalculateMetrics()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var metricDAOs = context.Metrics.AsNoTracking().Where(x => !x.IsDisabled).ToList();
                foreach (var metric in metricDAOs)
                {
                    MetricHelper.GetMetricCalculator(metric.MetricEnum).Calculate(context, _jira);
                }
                //TODO: calculate system scores here
            }
        }
    }
}
