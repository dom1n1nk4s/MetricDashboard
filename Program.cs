using MetricDashboard.Components;
using MetricDashboard.Components.Account;
using MetricDashboard.Data;
using MetricDashboard.Models;
using MetricDashboard.Scraper;
using MetricDashboard.Scraper.MetricScrapers;
using MetricDashboard.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace MetricDashboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddLocalization();
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
            builder.Services.AddHostedService<Worker>();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
            builder.Services.AddSingleton<JiraService>();
            builder.Services.AddSingleton<BitBucketService>();

            builder.Services.AddSingleton<IMetricCalculator, DeployFreqCalculator>();
            builder.Services.AddSingleton<IMetricCalculator, LeadTimeCalculator>();
            builder.Services.AddSingleton<IMetricCalculator, FailedRecoveryCalculator>();
            builder.Services.AddSingleton<IMetricCalculator, ChangeFailureCalculator>();
            builder.Services.AddSingleton<IMetricCalculator, SatSurveyCalculator>();
            builder.Services.AddSingleton<IMetricCalculator, WorkerRetRateCalculator>();
            builder.Services.AddSingleton<IMetricCalculator, BugCountCalculator>();
            builder.Services.AddSingleton<IMetricCalculator, ClientSatSurveyCalculator>();
            builder.Services.AddSingleton<IMetricCalculator, CodeTaskMergeCountCalculator>();
            builder.Services.AddSingleton<IMetricCalculator, CodeReviewPartCalculator>();
            builder.Services.AddSingleton<IMetricCalculator, TimeSpentWorkCalculator>();
            builder.Services.AddSingleton<IMetricCalculator, CodeIntegTimeCalculator>();
            builder.Services.AddSingleton<IMetricCalculator, OnboardingTimeCalculator>();
            builder.Services.AddSingleton<IMetricCalculator, TaskHandoverCalculator>();
            builder.Services.AddSingleton<IMetricCalculator, WorkflowIntCalculator>();
            builder.Services.AddSingleton<IMetricCalculator, BusinessValuePercCalculator>();

            builder.Services.AddRadzenComponents();
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
            {
                options.EnableSensitiveDataLogging(true);
                options.UseSqlServer(connectionString);
            });
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

            var app = builder.Build();

            string[] supportedCultures = ["en-US"];
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
            app.UseRequestLocalization(localizationOptions);
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            app.Run();
        }
    }
}
