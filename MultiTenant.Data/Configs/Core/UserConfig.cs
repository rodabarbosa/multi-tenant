using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenant.Data.Extensions;
using MultiTenant.Domain.Entities.Core;

namespace MultiTenant.Data.Configs.Core;

public class UserConfig : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.BaseConfig();

        builder.HasOne(e => e.Tenant)
            .WithMany(f => f.Users)
            .HasForeignKey(e => e.TenantId)
            .IsRequired();

        builder.Property(e => e.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(e => e.Email)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(e => e.Username)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Password)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(e => new { e.TenantId, e.Username })
            .IsUnique();
    }
}
