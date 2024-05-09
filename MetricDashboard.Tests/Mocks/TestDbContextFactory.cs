using MetricDashboard.Data;
using MetricDashboard.Data.Enums;
using MetricDashboard.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricDashboard.Tests.Mocks
{
    public class TestDbContextFactory : IDbContextFactory<ApplicationDbContext>
    {
        public DbContextOptions<ApplicationDbContext> Options;
        private static bool isSeeded = false;

        public TestDbContextFactory(string databaseName = "InMemoryTest")
        {
            Options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        public ApplicationDbContext CreateDbContext()
        {
            var context = new ApplicationDbContext(Options);
            if (!isSeeded)
            {
                isSeeded = true;
                context.GlobalMetricSettings.Add(new GlobalMetricSettings
                { Id = 1, Scope = TimeScopeEnum.SIX_MONTHS, SprintLength = 14, GreenCalculationValue = 10, YellowCalculationValue = 5, RedCalculationValue = 1 });
                context.SaveChanges();
            }
            return context;
        }
    }

}
