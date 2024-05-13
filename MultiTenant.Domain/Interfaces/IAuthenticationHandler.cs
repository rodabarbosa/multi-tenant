using MultiTenant.Domain.Models;

namespace MultiTenant.Domain.Interfaces;

public interface IAuthenticationHandler
{
    Task<Response<Account>> ExecuteAsync(AuthenticationRequest request, CancellationToken cancellationToken);
}
