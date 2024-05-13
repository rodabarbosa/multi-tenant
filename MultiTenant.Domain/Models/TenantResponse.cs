namespace MultiTenant.Domain.Models;

public class TenantResponse
{
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = default!;
}
