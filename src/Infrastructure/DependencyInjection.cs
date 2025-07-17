using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Constants;
using TropicFeel.Infrastructure.Data;
using TropicFeel.Infrastructure.Data.Interceptors;
using TropicFeel.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using TropicFeel.Infrastructure.Netsuit.Authentication;
using Microsoft.Extensions.Options;
using TropicFeel.Infrastructure.Common;
using TropicFeel.Infrastructure.Hangfire;
using TropicFeel.Infrastructure.Options;
using TropicFeel.Infrastructure.Sprint;
using TropicFeel.Infrastructure.JLP;
using TropicFeel.Infrastructure.Netsuit;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        
        
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();
        
        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();
        

        //Add Options
        services.Configure<NetSuiteConfig>(configuration.GetRequiredSection("NetSuiteConfig"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<NetSuiteConfig>>().Value);

        services.Configure<SprintConfig>(configuration.GetRequiredSection("SprintConfig"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<SprintConfig>>().Value);

        services.Configure<JlpConfig>(configuration.GetRequiredSection("JLPConfig"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<JlpConfig>>().Value);
        
        services.Configure<AppConfig>(configuration.GetRequiredSection("AppConfig"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<AppConfig>>().Value);

        services.AddScoped<IOAuthAuthenticationService, OAuthAuthenticationService>();
        services.AddSingleton<AuthorizationServiceFactory>();

        services.AddScoped<ISprintRequestService, SprintRequestService>();
        services.AddScoped<IJlpRequestService, JlpRequestService>();
        services.AddScoped<IExternalRequestClient, ExternalRequestClient>();
        services.AddScoped<INetsuitClientService, NetsuitClientService>();
        services.AddScoped<IJobScheduler, HangfireJobScheduler>();
        services.AddMemoryCache();

        services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        return services;
    }
}
