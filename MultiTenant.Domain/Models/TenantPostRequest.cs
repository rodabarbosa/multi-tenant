using System.ComponentModel.DataAnnotations;

namespace MultiTenant.Domain.Models;

public class TenantPostRequest
{
    [Required] [Length(3, 250)] public string TenantName { get; set; } = string.Empty;
    [Required] [Length(3, 50)] public string SchemaName { get; set; } = string.Empty;
}
