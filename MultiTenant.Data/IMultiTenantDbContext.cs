namespace MultiTenant.Data;

public interface IMultiTenantDbContext
{
    public string Schema { get; }
}
