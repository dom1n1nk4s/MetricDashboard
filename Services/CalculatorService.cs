using MetricDashboard.Data;
using MetricDashboard.Scraper.MetricScrapers;
using MetricDashboard.Scraper;
using Microsoft.EntityFrameworkCore;

namespace MetricDashboard.Services
{
    public class CalculatorService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly IEnumerable<IMetricCalculator> _calculators;
        public bool IsRunning { get; private set; }

        public CalculatorService(ILogger<Worker> logger, IEnumerable<IMetricCalculator> calculators, IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _logger = logger;
            _calculators = calculators;
            _dbFactory = dbFactory;
        }

        public async Task Run()
        {
            if (IsRunning)
            {
                return;
            }

            IsRunning = true;

            await CalculateMetrics();

            IsRunning = false;
        }
        private async Task CalculateMetrics()
        {
            using (var context = _dbFactory.CreateDbContext())
            {
                var metricDAOs = (await context.Metrics.AsNoTracking().ToListAsync()).OrderBy(x => x.MetricEnum).ToList();
                foreach (var calc in _calculators.ToList().OrderBy(x => x.MetricEnum).Zip(metricDAOs))
                {
                    if (!calc.Second.IsDisabled)
                    {
                        await calc.First.Calculate();
                    }
                }
                //TODO: calculate system scores here
            }
        }
    }
}
