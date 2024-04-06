using Atlassian.Jira;
using MetricDashboard.Data;
using MetricDashboard.Scraper.MetricScrapers;
using MetricDashboard.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MetricDashboard.Scraper
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly IEnumerable<IMetricCalculator> _calculators;

        public Worker(ILogger<Worker> logger, IEnumerable<IMetricCalculator> calculators, IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _logger = logger;
            _calculators = calculators;
            _dbFactory = dbFactory;
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
            using (var context = _dbFactory.CreateDbContext())
            {
                var metricDAOs = context.Metrics.AsNoTracking().AsEnumerable().OrderBy(x => x.MetricEnum).ToList();
                foreach (var calc in _calculators.ToList().OrderBy(x => x.MetricEnum).Zip(metricDAOs))
                {
                    if (!calc.Second.IsDisabled)
                    {
                        calc.First.Calculate();
                    }
                }
                //TODO: calculate system scores here
            }
        }
    }
}
