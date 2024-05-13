namespace MultiTenant.Domain.Entities.Core;

public class Tenant : TEntity
{
    public string Name { get; set; } = string.Empty;
    public string Schema { get; set; } = string.Empty;
    public virtual ICollection<User> Users { get; set; } = default!;
}
