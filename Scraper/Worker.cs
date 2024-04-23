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
        private readonly CalculatorService _calculatorService;

        public Worker(ILogger<Worker> logger, CalculatorService calculatorService)
        {
            _logger = logger;
            _calculatorService = calculatorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await _calculatorService.Run();
                await Task.Delay(60 * 60 * 1000, stoppingToken); // 60 minutes
            }

        }
    }
}
