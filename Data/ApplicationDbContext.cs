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
        public virtual DbSet<MetricResult> MetricResults { get; set; }
        public virtual DbSet<RadialSettings> RadialSettings { get; set; }
        public virtual DbSet<ColorRange> ColorRanges { get; set; }
        public virtual DbSet<GlobalMetricSettings> GlobalMetricSettings { get; set; }
        public virtual DbSet<SystemResult> SystemResults { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Metric>()
                .HasKey(x => x.Id);

            builder.Entity<Metric>()
                .HasOne(x => x.RadialSettings)
                .WithOne()
                .HasForeignKey<RadialSettings>(x => x.MetricId)
                .IsRequired();

            builder.Entity<RadialSettings>()
                .HasKey(x => x.Id);

            builder.Entity<ColorRange>()
                .HasKey(x => x.Id);

            builder.Entity<RadialSettings>()
                .HasMany(x => x.ColorRanges)
                .WithOne()
                .HasForeignKey(x => x.RadialSettingsId);


            SeedData(builder);
        }
        private void SeedData(ModelBuilder builder)
        {
            builder.Entity<Metric>().HasData(
            [
                new Metric { Id = 1, System = MetricSystemEnum.DORA, MetricEnum = MetricEnum.DEPLOYMENT_FREQUENCY },
                new Metric { Id = 2, System = MetricSystemEnum.DORA, MetricEnum = MetricEnum.LEAD_TIME_FOR_CHANGES },
                new Metric { Id = 3, System = MetricSystemEnum.DORA, MetricEnum = MetricEnum.FAILED_DEPLOYMENT_RECOVERY_TIME },
                new Metric { Id = 4, System = MetricSystemEnum.DORA, MetricEnum = MetricEnum.CHANGE_FAILURE_RATE },

                // Satisfaction
                new Metric { Id = 5, System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.SATISFACTION_SURVEY },
                new Metric { Id = 6, System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.WORKER_RETENTION_RATE },

                // Performance
                new Metric { Id = 7, System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.BUG_COUNT },
                new Metric { Id = 8, System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.CLIENT_SATISFACTION_SURVEY },

                // Activity
                new Metric { Id = 9, System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.CODE_TASK_MERGE_COMMIT_COUNT },
                new Metric { Id = 10, System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.CODE_REVIEW_PARTICIPATION },
                new Metric { Id = 11, System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.TIME_SPENT_WORKING },

                // Communication and Collaboration
                new Metric { Id = 12, System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.CODE_INTEGRATION_TIME },
                new Metric { Id = 13, System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.ONBOARDING_TIME },

                // Efficiency and Flow
                new Metric { Id = 14, System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.TASK_HANDOVERS_BEFORE_COMPLETION },
                new Metric { Id = 15, System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.WORKFLOW_INTERRUPTION_TIME },
                new Metric { Id = 16, System = MetricSystemEnum.SPACE, MetricEnum = MetricEnum.BUSINESS_VALUE_PERCENTAGE }
            ]);

            builder.Entity<GlobalMetricSettings>().HasData(
            [
                new GlobalMetricSettings
                { Id = 1, Scope = TimeScopeEnum.SIX_MONTHS, SprintLength = 14, GreenCalculationValue = 10, YellowCalculationValue = 5, RedCalculationValue = 1 }
            ]);
            //TODO: export db and run sql scripts for seeding radialsettings and color ranges. cant be bothered to do it in c#
        }
    }
}
