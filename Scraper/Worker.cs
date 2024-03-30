using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MetricDashboard.Scraper
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
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
                    MetricHelper.GetMetricCalculator(metric.MetricEnum).Calculate(context);
                }
            }

            //var jira = Jira.CreateRestClient("https://productivity-metrics-testing.atlassian.net/", "domininkas.stankevicius@stud.vilniustech.lt", "ATATT3xFfGF0VZCGbZ6lJtnU6AwllpPSGffV0TG-8Df1yrGiLelGQF4RZpo7lOCv8x82zC8DCsexT4CMBQi7gj3Cxf8ygtnYLyXRVC9EN3ifz6X-cSUHUQxoNMD4o5a0V9LLU9APOsShkFVUSAN_wKtnpqKNpGHuXVF5YqH9EAcKU9J2IZS4X3E=277107C6");
            //var issues = jira.Issues.Queryable.ToArray();
        }
    }
}
