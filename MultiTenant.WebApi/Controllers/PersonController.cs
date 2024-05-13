using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenant.Domain.Interfaces;
using MultiTenant.Domain.Models;
using MultiTenant.WebApi.Controllers.Base;

namespace MultiTenant.WebApi.Controllers;

[Authorize]
public class PersonController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(Response<PersonResponse[]>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync([FromServices] IPersonGetAllHandler handler,
        string? param,
        CancellationToken cancellationToken)
    {
        var result = await handler.ExecuteAsync(param, cancellationToken);
        return result.Success
            ? Ok(result)
            : BadRequest(result);
    }
}
