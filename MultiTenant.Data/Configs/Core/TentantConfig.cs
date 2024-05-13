using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenant.Data.Extensions;
using MultiTenant.Domain.Entities.Core;

namespace MultiTenant.Data.Configs.Core;

public class TentantConfig : IEntityTypeConfiguration<Tenant>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.BaseConfig();

        builder.Property(e => e.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(e => e.Schema)
            .HasMaxLength(50)
            .IsRequired();
    }
}
