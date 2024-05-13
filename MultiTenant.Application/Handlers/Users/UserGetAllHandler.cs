using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MultiTenant.Data;
using MultiTenant.Domain.Interfaces;
using MultiTenant.Domain.Models;
using System.Net;

namespace MultiTenant.Application.Handlers.Users;

public sealed class UserGetAllHandler(
    ILogger<UserGetAllHandler> logger,
    CoreContext context)
    : IUserGetAllHandler
{
    /// <inheritdoc />
    public async Task<Response<UserResponse[]>> ExecuteAsync(string? param, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting people from the database");

        var users = await context.Users
            .Select(x => new UserResponse
            {
                UserId = x.Id,
                Username = x.Username,
                Name = x.Name,
                TenantId = x.TenantId,
                CreatedAt = x.CreatedAt
            })
            .ToArrayAsync(cancellationToken);

        return new Response<UserResponse[]>(true,
            HttpStatusCode.OK,
            users,
            "Success");
    }
}
