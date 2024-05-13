using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MultiTenant.Data.Libs;

internal class BusinessCacheKeyFactory : IModelCacheKeyFactory
{
    public object Create(DbContext context, bool designTime)
    {
        return new BusinessCacheKey(context, designTime);
    }
}
