using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenant.Domain.Interfaces;
using MultiTenant.Domain.Models;
using MultiTenant.WebApi.Controllers.Base;

namespace MultiTenant.WebApi.Controllers;

[Authorize]
public class UserController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(Response<UserResponse[]>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync([FromServices] IUserGetAllHandler handler,
        string? param,
        CancellationToken cancellationToken)
    {
        var result = await handler.ExecuteAsync(param, cancellationToken);
        return result.Success
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Response<UserResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> PostAsync([FromServices] IUserPostHandler handler,
        [FromBody] UserPostRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.ExecuteAsync(request, cancellationToken);

        return result.Success
            ? Created(Request.Path, result)
            : BadRequest(result);
    }
}
