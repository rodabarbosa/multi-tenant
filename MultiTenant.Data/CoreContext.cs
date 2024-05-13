using Microsoft.EntityFrameworkCore;
using MultiTenant.Data.Configs.Core;
using MultiTenant.Domain.Entities.Core;

namespace MultiTenant.Data;

public class CoreContext(DbContextOptions<CoreContext> options)
    : DbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CoreContext).Assembly,
            t => t.Namespace == typeof(TentantConfig).Namespace
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}
