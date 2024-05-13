namespace MultiTenant.Domain.Entities.Core;

public class User : TEntity
{
    public Guid TenantId { get; set; }
    public virtual Tenant? Tenant { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}