using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MultiTenant.Data;
using MultiTenant.Domain.Interfaces;
using MultiTenant.Domain.Models;
using System.Net;

namespace MultiTenant.Application.Handlers.Persons;

public sealed class PersonGetAllHandler(ILogger<PersonGetAllHandler> logger, BusinessContext context)
    : IPersonGetAllHandler
{
    /// <inheritdoc />
    public async Task<Response<PersonResponse[]>> ExecuteAsync(string? param, CancellationToken cancellationToken)
    {
        logger.LogInformation("Gettting persons from tentant schema {tenant}", context.Schema);

        var people = await context.People
            .Select(x => new PersonResponse
            {
                PersonId = x.Id,
                Name = x.Name
            })
            .ToArrayAsync(cancellationToken);

        return new Response<PersonResponse[]>(true,
            HttpStatusCode.OK,
            people,
            "Success");
    }
}
