using MultiTenant.Domain.Models;

namespace MultiTenant.Domain.Interfaces;

public interface IPersonGetAllHandler
{
    Task<Response<PersonResponse[]>> ExecuteAsync(string? param, CancellationToken cancellationToken);
}
