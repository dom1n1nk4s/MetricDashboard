using MetricDashboard.Data.Enums;
using MetricDashboard.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
            builder.Entity<RadialSettings>().HasData(
                new RadialSettings
                {
                    Id = 5,
                    MetricEnum = MetricEnum.TIME_SPENT_WORKING,
                    Start = 0,
                    End = 20,
                    Step = 5,
                    PointerUnits = "days",
                    MetricId = 13
                },
                new RadialSettings
                {
                    Id = 6,
                    MetricEnum = MetricEnum.BUG_COUNT,
                    Start = 0,
                    End = 50,
                    Step = 5,
                    PointerUnits = "bugs",
                    MetricId = 7
                },
                new RadialSettings
                {
                    Id = 7,
                    MetricEnum = MetricEnum.BUSINESS_VALUE_PERCENTAGE,
                    Start = 0,
                    End = 100,
                    Step = 10,
                    PointerUnits = "%",
                    MetricId = 16
                },
                new RadialSettings
                {
                    Id = 8,
                    MetricEnum = MetricEnum.CHANGE_FAILURE_RATE,
                    Start = 0,
                    End = 100,
                    Step = 10,
                    PointerUnits = "%",
                    MetricId = 4
                },
                new RadialSettings
                {
                    Id = 9,
                    MetricEnum = MetricEnum.WORKER_RETENTION_RATE,
                    Start = 0,
                    End = 10,
                    Step = 1,
                    PointerUnits = "",
                    MetricId = 8
                },
                new RadialSettings
                {
                    Id = 10,
                    MetricEnum = MetricEnum.CODE_TASK_MERGE_COMMIT_COUNT,
                    Start = 0,
                    End = 36,
                    Step = 6,
                    PointerUnits = "h",
                    MetricId = 12
                },
                new RadialSettings
                {
                    Id = 11,
                    MetricEnum = MetricEnum.CODE_REVIEW_PARTICIPATION,
                    Start = 0,
                    End = 100,
                    Step = 10,
                    PointerUnits = "%",
                    MetricId = 10
                },
                new RadialSettings
                {
                    Id = 12,
                    MetricEnum = MetricEnum.DEPLOYMENT_FREQUENCY,
                    Start = 0,
                    End = 180,
                    Step = 20,
                    PointerUnits = "deployments",
                    MetricId = 1
                },
                new RadialSettings
                {
                    Id = 13,
                    MetricEnum = MetricEnum.LEAD_TIME_FOR_CHANGES,
                    Start = 0,
                    End = 36,
                    Step = 6,
                    PointerUnits = "h",
                    MetricId = 3
                },
                new RadialSettings
                {
                    Id = 14,
                    MetricEnum = MetricEnum.LEAD_TIME_FOR_CHANGES,
                    Start = 0,
                    End = 14,
                    Step = 1,
                    PointerUnits = "days",
                    MetricId = 2
                },
                new RadialSettings
                {
                    Id = 15,
                    MetricEnum = MetricEnum.CHANGE_FAILURE_RATE,
                    Start = 0,
                    End = 10,
                    Step = 1,
                    PointerUnits = "",
                    MetricId = 5
                },
                new RadialSettings
                {
                    Id = 16,
                    MetricEnum = MetricEnum.WORKFLOW_INTERRUPTION_TIME,
                    Start = 0,
                    End = 10,
                    Step = 1,
                    PointerUnits = "h",
                    MetricId = 15
                },
                new RadialSettings
                {
                    Id = 17,
                    MetricEnum = MetricEnum.CLIENT_SATISFACTION_SURVEY,
                    Start = 0,
                    End = 5000,
                    Step = 500,
                    PointerUnits = "objects",
                    MetricId = 9
                },
                new RadialSettings
                {
                    Id = 18,
                    MetricEnum = MetricEnum.TASK_HANDOVERS_BEFORE_COMPLETION,
                    Start = 0,
                    End = 5,
                    Step = 1,
                    PointerUnits = "handovers",
                    MetricId = 14
                },
                new RadialSettings
                {
                    Id = 19,
                    MetricEnum = MetricEnum.ONBOARDING_TIME,
                    Start = 0,
                    End = 72,
                    Step = 4,
                    PointerUnits = "h",
                    MetricId = 11
                },
                new RadialSettings
                {
                    Id = 20,
                    MetricEnum = MetricEnum.SATISFACTION_SURVEY,
                    Start = 0,
                    End = 60,
                    Step = 6,
                    PointerUnits = "months",
                    MetricId = 6
                }
            );
            builder.Entity<ColorRange>().HasData(
                new ColorRange
                {
                    Id = 15,
                    From = 0,
                    To = 5,
                    RadialSettingsId = 5,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 16,
                    From = 5,
                    To = 10,
                    RadialSettingsId = 5,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 17,
                    From = 10,
                    To = 20,
                    RadialSettingsId = 5,
                    Color = ColorEnum.RED
                },
                new ColorRange
                {
                    Id = 18,
                    From = 0,
                    To = 12,
                    RadialSettingsId = 6,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 19,
                    From = 12,
                    To = 30,
                    RadialSettingsId = 6,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 20,
                    From = 30,
                    To = 50,
                    RadialSettingsId = 6,
                    Color = ColorEnum.RED
                },
                new ColorRange
                {
                    Id = 21,
                    From = 90,
                    To = 100,
                    RadialSettingsId = 7,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 22,
                    From = 75,
                    To = 90,
                    RadialSettingsId = 7,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 23,
                    From = 0,
                    To = 75,
                    RadialSettingsId = 7,
                    Color = ColorEnum.RED
                },
                new ColorRange
                {
                    Id = 24,
                    From = 0,
                    To = 2,
                    RadialSettingsId = 8,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 25,
                    From = 2,
                    To = 10,
                    RadialSettingsId = 8,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 26,
                    From = 10,
                    To = 100,
                    RadialSettingsId = 8,
                    Color = ColorEnum.RED
                },
                new ColorRange
                {
                    Id = 27,
                    From = 8,
                    To = 10,
                    RadialSettingsId = 9,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 28,
                    From = 6,
                    To = 8,
                    RadialSettingsId = 9,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 29,
                    From = 0,
                    To = 6,
                    RadialSettingsId = 9,
                    Color = ColorEnum.RED
                },
                new ColorRange
                {
                    Id = 30,
                    From = 0,
                    To = 3,
                    RadialSettingsId = 10,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 31,
                    From = 3,
                    To = 8,
                    RadialSettingsId = 10,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 32,
                    From = 8,
                    To = 36,
                    RadialSettingsId = 10,
                    Color = ColorEnum.RED
                },
                new ColorRange
                {
                    Id = 33,
                    From = 90,
                    To = 100,
                    RadialSettingsId = 11,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 34,
                    From = 75,
                    To = 90,
                    RadialSettingsId = 11,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 35,
                    From = 0,
                    To = 75,
                    RadialSettingsId = 11,
                    Color = ColorEnum.RED
                },
                new ColorRange
                {
                    Id = 36,
                    From = 24,
                    To = 180,
                    RadialSettingsId = 12,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 37,
                    From = 6,
                    To = 24,
                    RadialSettingsId = 12,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 38,
                    From = 0,
                    To = 6,
                    RadialSettingsId = 12,
                    Color = ColorEnum.RED
                },
                new ColorRange
                {
                    Id = 39,
                    From = 0,
                    To = 1,
                    RadialSettingsId = 13,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 40,
                    From = 1,
                    To = 4,
                    RadialSettingsId = 13,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 41,
                    From = 4,
                    To = 36,
                    RadialSettingsId = 13,
                    Color = ColorEnum.RED
                },
                new ColorRange
                {
                    Id = 42,
                    From = 0,
                    To = 1,
                    RadialSettingsId = 14,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 43,
                    From = 1,
                    To = 3,
                    RadialSettingsId = 14,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 44,
                    From = 3,
                    To = 14,
                    RadialSettingsId = 14,
                    Color = ColorEnum.RED
                },
                new ColorRange
                {
                    Id = 45,
                    From = 8,
                    To = 10,
                    RadialSettingsId = 15,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 46,
                    From = 6,
                    To = 8,
                    RadialSettingsId = 15,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 47,
                    From = 0,
                    To = 6,
                    RadialSettingsId = 15,
                    Color = ColorEnum.RED
                },
                new ColorRange
                {
                    Id = 48,
                    From = 0,
                    To = 1,
                    RadialSettingsId = 16,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 49,
                    From = 1,
                    To = 3,
                    RadialSettingsId = 16,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 50,
                    From = 3,
                    To = 10,
                    RadialSettingsId = 16,
                    Color = ColorEnum.RED
                },
                new ColorRange
                {
                    Id = 51,
                    From = 3000,
                    To = 5000,
                    RadialSettingsId = 17,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 52,
                    From = 1500,
                    To = 3000,
                    RadialSettingsId = 17,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 53,
                    From = 0,
                    To = 1500,
                    RadialSettingsId = 17,
                    Color = ColorEnum.RED
                },
                new ColorRange
                {
                    Id = 54,
                    From = 0,
                    To = 1,
                    RadialSettingsId = 18,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 55,
                    From = 1,
                    To = 2,
                    RadialSettingsId = 18,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 56,
                    From = 2,
                    To = 5,
                    RadialSettingsId = 18,
                    Color = ColorEnum.RED
                },
                new ColorRange
                {
                    Id = 57,
                    From = 0,
                    To = 16,
                    RadialSettingsId = 19,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 58,
                    From = 16,
                    To = 36,
                    RadialSettingsId = 19,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 59,
                    From = 36,
                    To = 72,
                    RadialSettingsId = 19,
                    Color = ColorEnum.RED
                },
                new ColorRange
                {
                    Id = 60,
                    From = 24,
                    To = 60,
                    RadialSettingsId = 20,
                    Color = ColorEnum.GREEN
                },
                new ColorRange
                {
                    Id = 61,
                    From = 12,
                    To = 24,
                    RadialSettingsId = 20,
                    Color = ColorEnum.YELLOW
                },
                new ColorRange
                {
                    Id = 62,
                    From = 0,
                    To = 12,
                    RadialSettingsId = 20,
                    Color = ColorEnum.RED
                }
            );
        }
    }
}
