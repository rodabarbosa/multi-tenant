using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using MultiTenant.Data.Configs.Business;
using MultiTenant.Data.Libs;
using MultiTenant.Domain.Entities.Business;

namespace MultiTenant.Data;

public class BusinessContext
    : DbContext, IBusinessContext
{
    private const string DefaultSchema = "public";
    private readonly string _connnectionString;

    public BusinessContext(IConfiguration configuration, DbContextOptions<BusinessContext> options)
        : base(options)
    {
        Schema = "dev";
        _connnectionString = configuration.GetConnectionString("DefaultConnection")
                             ?? throw new Exception("Connection string not found");
    }

    public BusinessContext(IConfiguration configuration, string schema)
    {
        if (string.IsNullOrEmpty(schema))
            throw new ArgumentNullException(nameof(schema));

        if (schema.Equals(DefaultSchema, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Invalid schema name", nameof(schema));

        Schema = schema;
        _connnectionString = configuration.GetConnectionString("DefaultConnection")
                             ?? throw new Exception("Connection string not found");
    }

    public DbSet<Person> People { get; set; } = default!;
    public string Schema { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BusinessContext).Assembly,
            t => t.Namespace == typeof(PersonConfig).Namespace
        );

        // modelBuilder.Entity<Person>(x =>
        // {
        //     x.Metadata.SetSchema(Schema);
        //     x.Metadata.SetIsTableExcludedFromMigrations(true);
        // });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        optionsBuilder
            .ReplaceService<IMigrationsAssembly, BusinessAssembly>()
            .ReplaceService<IModelCacheKeyFactory, BusinessCacheKeyFactory>();

        optionsBuilder.UseNpgsql(
                _connnectionString,
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", Schema)
                    .CommandTimeout(90)
            )
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();
    }
}
