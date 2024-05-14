using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenant.Domain.Entities;

namespace MultiTenant.Data.Extensions;

public static class ConfigExtension
{
    public static void BaseConfig<T>(this EntityTypeBuilder<T> builder)
        where T : TEntity
    {
        builder.HasKey(e => e.Id);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.Property(x => x.DeletedAt);

        builder.Ignore(x => x.IsDeleted);
    }

    public static void BusinessConfig<T>(this EntityTypeBuilder<T> builder)
        where T : TEntity
    {
        builder.BaseConfig();

        //builder.Metadata.SetIsTableExcludedFromMigrations(true);
    }
}
