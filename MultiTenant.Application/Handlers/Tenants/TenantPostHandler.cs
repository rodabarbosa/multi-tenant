using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MultiTenant.Application.Extensions;
using MultiTenant.Application.Utils;
using MultiTenant.Data;
using MultiTenant.Domain.Entities.Core;
using MultiTenant.Domain.Interfaces;
using MultiTenant.Domain.Models;
using System.Net;

namespace MultiTenant.Application.Handlers.Tenants;

public sealed class TenantPostHandler(
    ILogger<TenantPostHandler> logger,
    CoreContext context)
    : BaseHandler, ITenantPostHandler
{
    /// <inheritdoc />
    public async Task<Response<TenantResponse>> ExecuteAsync(TenantPostRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("registering new tenant: {tenant}", request.ToJson());

        var isValid = ValidationUtil.Validate(request, out var errors);
        if (!isValid)
            return GetValidationResponse<TenantResponse>(errors, logger);

        var tenantWithSameSchema = await context.Tenants
            .Where(x => EF.Functions.ILike(x.Schema, request.SchemaName))
            .FirstOrDefaultAsync(cancellationToken);

        if (tenantWithSameSchema is not null)
        {
            logger.LogError("Tenant with same schema name already exists: {tenant}", request.ToJson());

            return new Response<TenantResponse>(false,
                HttpStatusCode.BadRequest,
                default,
                "Tenant with same schema name already exists");
        }

        try

        {
            var entity = new Tenant
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Name = request.TenantName,
                Schema = request.SchemaName
            };

            await context.Tenants
                .AddAsync(entity, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            var response = new TenantResponse
            {
                TenantId = entity.Id,
                TenantName = entity.Name,
                CreatedAt = entity.CreatedAt
            };

            return new Response<TenantResponse>(true,
                HttpStatusCode.Created,
                response,
                "Sucess");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error registering new tenant: {tenant}", request.ToJson());
            return new Response<TenantResponse>(false,
                HttpStatusCode.BadRequest,
                default,
                e.Message);
        }
    }
}
