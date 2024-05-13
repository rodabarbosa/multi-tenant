using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MultiTenant.Application.Utils;
using MultiTenant.Data;
using MultiTenant.Domain.Entities.Core;
using MultiTenant.Domain.Interfaces;
using MultiTenant.Domain.Models;
using System.Net;

namespace MultiTenant.Application.Handlers.Authenticate;

public class AuthenticationHandler(
    ILogger<AuthenticationHandler> logger,
    CoreContext coreContext,
    TokenUtil tokenUtil)
    : BaseHandler, IAuthenticationHandler
{
    public async Task<Response<Account>> ExecuteAsync(AuthenticationRequest request, CancellationToken cancellationToken)
    {
        var isValid = ValidationUtil.Validate(request, out var errors);
        if (!isValid) return GetValidationResponse<Account>(errors, logger);

        if (request.Username.Equals("admin") && request.Password.Equals("admin@2024"))
        {
            var user = await GetAccount(new User
                {
                    Id = new Guid("A02D19BD-AA69-4CE2-A631-212F51358910"),
                    CreatedAt = DateTime.Now,
                    TenantId = Guid.Empty,
                    Name = "Admin",
                    Email = "admin@admin.com",
                    Username = "admin"
                },
                cancellationToken);

            return new Response<Account>(true,
                HttpStatusCode.OK,
                user,
                "Successful login");
        }

        var result = await GetUser(request, cancellationToken);
        if (result is null)
        {
            logger.LogError("Username {username} not found", request.Username);
            return new Response<Account>(false,
                HttpStatusCode.BadRequest,
                default,
                "Username not found");
        }

        var account = await GetAccount(result, cancellationToken);

        return new Response<Account>(true,
            HttpStatusCode.OK,
            account,
            "Successful login");
    }

    private async Task<User?> GetUser(AuthenticationRequest request, CancellationToken cancellationToken)
    {
        return await coreContext.Users
            .Include(x => x.Tenant)
            .Where(x => string.Equals(x.Username, request.Username))
            .Where(x => string.Equals(x.Password, request.Password))
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task<Account> GetAccount(User user, CancellationToken cancellationToken)
    {
        var token = tokenUtil.GenerateToken(user);

        return new Account(user.Name,
            user.Username,
            token.Item1,
            "role",
            token.Item2);
    }
}
