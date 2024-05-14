using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenant.Domain.Interfaces;
using MultiTenant.Domain.Models;
using MultiTenant.WebApi.Controllers.Base;

namespace MultiTenant.WebApi.Controllers;

[Authorize]
public class TenantController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(Response<TenantResponse[]>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync([FromServices] ITenantGetAllHandler handler,
        string? param,
        CancellationToken cancellationToken)
    {
        var result = await handler.ExecuteAsync(param, cancellationToken);
        return result.Success
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Response<TenantResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> PostAsync([FromServices] ITenantPostHandler handler,
        [FromBody] TenantPostRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.ExecuteAsync(request, cancellationToken);

        return result.Success
            ? Created(Request.Path, result)
            : BadRequest(result);
    }

    [HttpGet("/migrate")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> MigrateASync([FromServices] ITenantMigrateHandler handler, CancellationToken cancellationToken)
    {
        var response = await handler.ExecuteAsync(cancellationToken);

        return Ok(response);
    }
}
