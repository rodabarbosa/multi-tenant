using Microsoft.Extensions.Logging;
using MultiTenant.Application.Extensions;
using MultiTenant.Application.Utils;
using MultiTenant.Data;
using MultiTenant.Domain.Entities.Core;
using MultiTenant.Domain.Interfaces;
using MultiTenant.Domain.Models;
using System.Net;

namespace MultiTenant.Application.Handlers.Users;

public sealed class UserPostHandler(
    ILogger<UserPostHandler> logger,
    CoreContext context)
    : BaseHandler, IUserPostHandler
{
    /// <inheritdoc />
    public async Task<Response<UserResponse>> ExecuteAsync(UserPostRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("registering new user: {user}", request.ToJson());

        var isValid = ValidationUtil.Validate(request, out var errors);
        if (!isValid)
            return GetValidationResponse<UserResponse>(errors, logger);

        try
        {
            var entity = new User
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                TenantId = request.TenantId,
                Name = request.Name,
                Email = request.Email,
                Username = request.Username,
                Password = request.Password
            };

            await context.Users
                .AddAsync(new User(), cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            var response = new UserResponse
            {
                UserId = entity.Id,
                Username = entity.Username,
                Name = entity.Name,
                TenantId = entity.TenantId,
                CreatedAt = entity.CreatedAt
            };

            return new Response<UserResponse>(true,
                HttpStatusCode.Created,
                response,
                "Created");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while registering new user. {user}", request.ToJson());
            return new Response<UserResponse>(false,
                HttpStatusCode.BadRequest,
                default,
                e.Message);
        }
    }
}
