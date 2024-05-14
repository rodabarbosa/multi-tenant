using Microsoft.EntityFrameworkCore;
using MultiTenant.Data;
using MultiTenant.Domain.Interfaces;

namespace MultiTenant.WebApi.Extensions;

/// <summary>
/// ServiceCollection Extensions
/// </summary>
static public class DataProviderExtension
{
    private const string DefaultDevSchema = "dev";

    /// <summary>
    ///   Add Database configuration to services.
    /// </summary>
    /// <param name="builder"></param>
    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        var enableSensitiveData = true;
        //_ = bool.TryParse(builder.Configuration.GetSection("EnableSensitiveDataLogging")?.Value, out var enableSensitiveData);

        builder.Services
            .AddDbContextFactory<CoreContext>(options => options
                    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), options => options.CommandTimeout(90))
                    .EnableDetailedErrors(enableSensitiveData)
                    .EnableSensitiveDataLogging(enableSensitiveData)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution),
                ServiceLifetime.Transient);

        // builder.Services
        //     .AddDbContextFactory<BusinessContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), options =>
        //             {
        //                 options.CommandTimeout(90);
        //                 options.MigrationsHistoryTable("__EFMigrationsHistory", DefaultDevSchema);
        //             })
        //             .EnableDetailedErrors(enableSensitiveData)
        //             .EnableSensitiveDataLogging(enableSensitiveData)
        //             .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution),
        //         ServiceLifetime.Transient);

        builder.Services
            .AddScoped<BusinessContext>(x =>
            {
                var config = x.GetService<IConfiguration>() ?? throw new Exception("Could not find configurations");

                if (EF.IsDesignTime)
                    return new BusinessContext(config, new DbContextOptions<BusinessContext>());

                var httpContext = x.GetService<IHttpContextAccessor>() ?? throw new Exception("HTTP context not accessible");

                var coreContext = x.GetService<CoreContext>() ?? throw new Exception("HQ database not set");

                var schema = httpContext.HttpContext?.GetSchemaFromHeader(coreContext) ?? DefaultDevSchema;

                return new BusinessContext(config, schema);
            });

        return builder;
    }

    static private string? GetSchemaFromHeader(this HttpContext http, CoreContext context)
    {
        var username = http.User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            return default;

        return context.Users
            .Where(x => EF.Functions.ILike(x.Username, username))
            .Select(x => x.Tenant!.Schema)
            .FirstOrDefault();
    }

    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        var coreContext = scope.ServiceProvider.GetRequiredService<CoreContext>();
        coreContext.Database.Migrate();

        var migrateHandler = scope.ServiceProvider.GetRequiredService<ITenantMigrateHandler>();
        migrateHandler.ExecuteAsync(CancellationToken.None)
            .Wait();

        return app;
    }
}
