using System.ComponentModel.DataAnnotations;

namespace MultiTenant.Domain.Models;

public class UserPostRequest
{
    [Required] public Guid TenantId { get; set; }
    [Required] [Length(3, 50)] public string Username { get; set; } = string.Empty;
    [Required] [Length(3, 150)] public string Password { get; set; } = string.Empty;
    [Required] [Length(3, 150)] public string Email { get; set; } = string.Empty;
    [Required] [Length(3, 250)] public string Name { get; set; } = string.Empty;
}
