using MultiTenant.Domain.Models;

namespace MultiTenant.Domain.Interfaces;

public interface IUserPostHandler
{
    Task<Response<UserResponse>> ExecuteAsync(UserPostRequest request, CancellationToken cancellationToken);
}
