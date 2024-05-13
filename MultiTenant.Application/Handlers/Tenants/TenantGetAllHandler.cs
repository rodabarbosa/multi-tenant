using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MultiTenant.Data;
using MultiTenant.Domain.Interfaces;
using MultiTenant.Domain.Models;
using System.Net;

namespace MultiTenant.Application.Handlers.Tenants;

public sealed class TenantGetAllHandler(
    ILogger<TenantGetAllHandler> logger,
    CoreContext context)
    : ITenantGetAllHandler
{
    /// <inheritdoc />
    public async Task<Response<TenantResponse[]>> ExecuteAsync(string? param, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting tenants from the database.");

        var tenants = await context.Tenants
            .Select(x => new TenantResponse
            {
                TenantId = x.Id,
                TenantName = x.Name,
                CreatedAt = x.CreatedAt
            })
            .ToArrayAsync(cancellationToken);

        return new Response<TenantResponse[]>(true,
            HttpStatusCode.OK,
            tenants,
            "Success");
    }
}
