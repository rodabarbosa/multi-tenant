using Microsoft.EntityFrameworkCore;

namespace MultiTenant.Data.Libs;

internal class BusinessCacheKey
{
    private readonly Type _dbContextType;
    private readonly bool _designTime;
    private readonly string? _schema;

    public BusinessCacheKey(DbContext context, bool designTime)
    {
        _dbContextType = context.GetType();
        _designTime = designTime;
        _schema = (context as IMultiTenantDbContext)?.Schema;
    }

    protected bool Equals(BusinessCacheKey other)
    {
        return _dbContextType == other._dbContextType
               && _designTime == other._designTime
               && _schema == other._schema;
    }

    public override bool Equals(object? obj)
    {
        return obj is BusinessCacheKey otherAsKey && Equals(otherAsKey);
    }

    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(_dbContextType);
        hash.Add(_designTime);
        hash.Add(_schema);

        return hash.ToHashCode();
    }
}
