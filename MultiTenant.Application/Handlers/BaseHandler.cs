using Microsoft.Extensions.Logging;
using MultiTenant.Application.Extensions;
using MultiTenant.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace MultiTenant.Application.Handlers;

public abstract class BaseHandler
{
    protected Response<T> GetValidationResponse<T>(IEnumerable<ValidationResult> errors, ILogger logger)
    {
        logger.LogError("Ocorreram erros de validação. {errors}", errors.ToJson());

        var errorMessage = new StringBuilder("Ocorreram erros de validação");
        foreach (var error in errors)
        {
            errorMessage.AppendLine();
            errorMessage.Append(error.MemberNames.First())
                .Append(": ")
                .AppendLine(error.ErrorMessage);
        }

        return new Response<T>(false,
            HttpStatusCode.UnprocessableEntity,
            default,
            errorMessage.ToString());
    }
}
