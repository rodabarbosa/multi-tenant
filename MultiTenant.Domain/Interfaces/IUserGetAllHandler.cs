using MultiTenant.Domain.Models;

namespace MultiTenant.Domain.Interfaces;

public interface IUserGetAllHandler
{
    Task<Response<UserResponse[]>> ExecuteAsync(string? param, CancellationToken cancellationToken);
}
