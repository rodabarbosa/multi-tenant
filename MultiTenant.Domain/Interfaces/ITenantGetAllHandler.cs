using MultiTenant.Domain.Models;

namespace MultiTenant.Domain.Interfaces;

public interface ITenantGetAllHandler
{
    Task<Response<TenantResponse[]>> ExecuteAsync(string? param, CancellationToken cancellationToken);
}
