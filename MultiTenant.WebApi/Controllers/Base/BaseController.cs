using Microsoft.AspNetCore.Mvc;
using MultiTenant.Domain.Models;

namespace MultiTenant.WebApi.Controllers.Base;

[Produces("application/json")]
[Consumes("application/json")]
[ApiConventionType(typeof(DefaultApiConventions))]
[ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
[ApiController]
[Route("v{version:apiVersion}/[controller]")]
public abstract class BaseController : ControllerBase
{
}
