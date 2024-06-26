using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using System.Reflection;

namespace MultiTenant.Data.Libs;

public class BusinessAssembly(
    ICurrentDbContext currentContext,
    IDbContextOptions options,
    IMigrationsIdGenerator idGenerator,
    IDiagnosticsLogger<DbLoggerCategory.Migrations> logger)
    : MigrationsAssembly(currentContext, options, idGenerator, logger), IMigrationsAssembly
{
    private readonly DbContext _context = currentContext.Context;

    public override Migration CreateMigration(TypeInfo migrationClass, string activeProvider)
    {
        if (string.IsNullOrEmpty(activeProvider))
            throw new ArgumentNullException(nameof(activeProvider));

        var isBusinessMigration = migrationClass.GetConstructor(new[] { typeof(IMultiTenantDbContext) }) != null;
        if (isBusinessMigration && _context is IMultiTenantDbContext multiTenantContext)
        {
            var migration = (Migration?)Activator.CreateInstance(migrationClass.AsType(),
                multiTenantContext);

            if (migration != null)
            {
                migration.ActiveProvider = activeProvider;
                return migration;
            }
        }

        return base.CreateMigration(migrationClass, activeProvider);
    }
}
