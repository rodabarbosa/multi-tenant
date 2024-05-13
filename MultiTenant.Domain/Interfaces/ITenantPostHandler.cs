using MultiTenant.Domain.Models;

namespace MultiTenant.Domain.Interfaces;

public interface ITenantPostHandler
{
    Task<Response<TenantResponse>> ExecuteAsync(TenantPostRequest request, CancellationToken cancellationToken);
}
