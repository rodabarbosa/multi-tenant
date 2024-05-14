using MultiTenant.Domain.Models;

namespace MultiTenant.Domain.Interfaces;

public interface ITenantMigrateHandler
{
    Task<Response<bool>> ExecuteAsync(CancellationToken cancellationToken);
}
