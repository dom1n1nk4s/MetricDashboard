using MetricDashboard.Data.Enums;
using MetricDashboard.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MetricDashboard.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public virtual DbSet<Metric> Metrics { get; set; }
        public virtual DbSet<RadialSettings> RadialSettings { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Metric>()
                .HasKey(x => x.MetricEnum);

            builder.Entity<Metric>()
                .HasOne(x => x.RadialSettings)
                .WithOne()
                .HasForeignKey<RadialSettings>(x=>x.MetricEnum);              



            if (Database.GetPendingMigrations().Any())
            {
                SeedData(builder);
            }
        }
        private void SeedData(ModelBuilder builder)
        {
            builder.Entity<Metric>().HasData(
            [
                new Metric { System = MetricSystemEnum.DORA, MetricEnum = MetricEnum.DEPLOYMENT_FREQUENCY },
                new Metric { System = MetricSystemEnum.DORA, MetricEnum = MetricEnum.LEAD_TIME_FOR_CHANGES },
                new Metric { System = MetricSystemEnum.DORA, MetricEnum = MetricEnum.FAILED_DEPLOYMENT_RECOVERY_TIME },
                new Metric { System = MetricSystemEnum.DORA, MetricEnum = MetricEnum.CHANGE_FAILURE_RATE },

                // Satisfaction
                new Metric { System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.SATISFACTION_SURVEY },
                new Metric { System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.WORKER_RETENTION_RATE },

                // Performance
                new Metric { System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.BUG_COUNT },
                new Metric { System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.CLIENT_SATISFACTION_SURVEY },

                // Activity
                new Metric { System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.CODE_TASK_MERGE_COMMIT_COUNT },
                new Metric { System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.CODE_REVIEW_PARTICIPATION },
                new Metric { System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.TIME_SPENT_WORKING },

                // Communication and Collaboration
                new Metric { System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.CODE_INTEGRATION_TIME },
                new Metric { System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.ONBOARDING_TIME },

                // Efficiency and Flow
                new Metric { System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.TASK_HANDOVERS_BEFORE_COMPLETION },
                new Metric { System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.WORKFLOW_INTERRUPTION_TIME },
                new Metric { System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.BUSINESS_VALUE_PERCENTAGE }
            ]);
        }
    }
}
