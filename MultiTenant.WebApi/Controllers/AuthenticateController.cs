using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenant.Domain.Interfaces;
using MultiTenant.Domain.Models;
using MultiTenant.WebApi.Controllers.Base;

namespace MultiTenant.WebApi.Controllers;

public class AuthenticateController : BaseController
{
    /// <summary>
    /// Authenticate user
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="request">Authentication data</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Response<Account>), StatusCodes.Status200OK)]
    public async Task<IActionResult> PostAsync([FromServices] IAuthenticationHandler handler,
        [FromBody] AuthenticationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await handler.ExecuteAsync(request, cancellationToken);

        return response.Code switch
        {
            StatusCodes.Status400BadRequest => BadRequest(response),
            StatusCodes.Status404NotFound => NotFound(response),
            _ => Ok(response)
        };
    }
}
