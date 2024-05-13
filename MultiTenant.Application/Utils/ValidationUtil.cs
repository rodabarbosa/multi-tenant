using System.ComponentModel.DataAnnotations;

namespace MultiTenant.Application.Utils;

public class ValidationUtil
{
    /// <summary>
    /// Validates the given request object and returns the validation errors, if any.
    /// </summary>
    /// <typeparam name="T">The type of the request object.</typeparam>
    /// <param name="request">The request object to be validated.</param>
    /// <param name="errors">The array of validation errors, if any.</param>
    /// <returns>True if the request is valid, false otherwise.</returns>
    public static bool Validate<T>(T request, out ValidationResult[] errors)
    {
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(request!,
            new ValidationContext(request!),
            results,
            true);

        errors = results.ToArray();

        return isValid;
    }
}
