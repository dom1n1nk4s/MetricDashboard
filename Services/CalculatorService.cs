using MetricDashboard.Data;
using MetricDashboard.Scraper.MetricScrapers;
using MetricDashboard.Scraper;
using Microsoft.EntityFrameworkCore;
using MetricDashboard.Extensions;
using MetricDashboard.Data.Enums;
using MetricDashboard.Data.Models;

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
            try
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
                    var globalSettings = context.GlobalMetricSettings.First(x => x.Id == 1);
                    var metricSettings = context.RadialSettings.Include(x => x.ColorRanges).AsNoTracking().ToList();
                    var metricResults = context.MetricResults.AsNoTracking().GroupBy(x => x.MetricEnum)
                        .Select(group => group.OrderByDescending(y => y.Date).FirstOrDefault()).ToList();
                    var metricFinalScores = new List<(MetricSystemEnum systemEnum, int score)>();
                    foreach (var zip in metricResults.OrderBy(x => x.MetricEnum).Zip(metricSettings.OrderBy(x => x.MetricEnum), metricDAOs))
                    {
                        var metricResult = zip.First;
                        var metricSetting = zip.Second;
                        var metricDao = zip.Third;
                        var scoredColor = metricSetting.GetColor(metricResult.Score);
                        var systemScore = GetColorScore(scoredColor, globalSettings);
                        metricFinalScores.Add((metricDao.System, systemScore));
                    }
                    context.SystemResults.AddRange(metricFinalScores.GroupBy(x => x.systemEnum).Select(x => new SystemResult
                    {
                        SystemEnum = x.Key,
                        Score = x.Select(y => y.score).Average(),
                    }));
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        public static int GetColorScore(ColorEnum color, GlobalMetricSettings globalMetricSettings)
        {
            switch (color)
            {
                case ColorEnum.RED:
                    return globalMetricSettings.RedCalculationValue;
                case ColorEnum.YELLOW:
                    return globalMetricSettings.YellowCalculationValue;
                case ColorEnum.GREEN:
                    return globalMetricSettings.GreenCalculationValue;
                default:
                    throw new ArgumentException("Invalid color enum value.");
            }
        }
    }
}
