using Azure.Identity;
using Hangfire;
using Hangfire.SqlServer;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Infrastructure.Data;
using TropicFeel.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Service;
using NSwag;
using NSwag.Generation.Processors.Security;
using TropicFeel.Application.Jobs;
using TropicFeel.Infrastructure.Options;
using TropicFeel.Infrastructure.Sprint;
using ZymLabs.NSwag.FluentValidation;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddRazorPages();

        services.AddScoped(provider =>
        {
            var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
            var loggerFactory = provider.GetService<ILoggerFactory>();

            return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
        });
        services.AddSingleton<IAppConfigService, AppConfigService>();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();

        services.AddOpenApiDocument((configure, sp) =>
        {
            configure.Title = "TropicFeel API";


            // Add the fluent validations schema processor
            var fluentValidationSchemaProcessor = 
                sp.CreateScope().ServiceProvider.GetRequiredService<FluentValidationSchemaProcessor>();

            configure.SchemaSettings.SchemaProcessors.Add(fluentValidationSchemaProcessor);

            // Add JWT
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");
        
        services.AddHangfire(globalConfiguration => globalConfiguration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        services.AddHangfireServer();

        var recurringJobManager = services.BuildServiceProvider().GetRequiredService<IRecurringJobManager>();
        ConfigureRecurringJobs(recurringJobManager);
        
        return services;
    }

    public static IServiceCollection AddKeyVaultIfConfigured(this IServiceCollection services, ConfigurationManager configuration)
    {
        var keyVaultUri = configuration["KeyVaultUri"];
        if (!string.IsNullOrWhiteSpace(keyVaultUri))
        {
            configuration.AddAzureKeyVault(
                new Uri(keyVaultUri),
                new DefaultAzureCredential());
        }

        return services;
    }
    private static void ConfigureRecurringJobs(IRecurringJobManager recurringJobManager)
    {
        //recurringJobManager.AddOrUpdate<ReturnJob>("returnJob", x => x.Execute(null), "*/30 * * * *");
        recurringJobManager.AddOrUpdate<TrackingJob>("tracking", x => x.ExecuteTracking(null), "0 */2 * * *");
        recurringJobManager.AddOrUpdate<DispatchJob>("Dispatch", x => x.ExecuteDispatch(null), "0 */3 * * *");

        recurringJobManager.AddOrUpdate<CreateOrderJob>("createOrderJob", x => x.ExecuteOrder(null), "0 */1 * * *");
        recurringJobManager.AddOrUpdate<InvoiceOrderJob>("invoiceOrderJob", x => x.Execute(null), "0 0 */15 * *");
    }
}
