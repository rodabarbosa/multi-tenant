using System.ComponentModel.DataAnnotations;

namespace MultiTenant.Domain.Models;

public class AuthenticationRequest(string username, string password)
{
    public AuthenticationRequest()
        : this(string.Empty, string.Empty)
    {
    }

    [Required] [Length(3, 20)] public string Username { get; set; } = username.Trim();

    [Required] [Length(3, 50)] public string Password { get; set; } = password.Trim();
}
