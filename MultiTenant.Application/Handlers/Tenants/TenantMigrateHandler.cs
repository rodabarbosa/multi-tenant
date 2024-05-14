using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MultiTenant.Data;
using MultiTenant.Domain.Entities.Core;
using MultiTenant.Domain.Interfaces;
using MultiTenant.Domain.Models;
using System.Net;

namespace MultiTenant.Application.Handlers.Tenants;

public sealed class TenantMigrateHandler(
    ILogger<TenantMigrateHandler> logger,
    IConfiguration configuration,
    CoreContext context)
    : BaseHandler, ITenantMigrateHandler
{
    /// <inheritdoc />
    public async Task<Response<bool>> ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("migrating tenants");

        var tenants = await context.Tenants
            .ToListAsync(cancellationToken);

        var list = tenants
            .Select(x => MigrateTenant(x, cancellationToken));

        await Task.WhenAll(list);

        return new Response<bool>(true, HttpStatusCode.OK, true, "Success");
    }

    private Task MigrateTenant(Tenant tenant, CancellationToken cancellationToken)
    {
        using var businessContext = new BusinessContext(configuration, tenant.Schema);

        return businessContext.Database.MigrateAsync(cancellationToken);
    }
}
