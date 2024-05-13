using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenant.Data.Extensions;
using MultiTenant.Domain.Entities.Business;

namespace MultiTenant.Data.Configs.Business;

public class PersonConfig : IEntityTypeConfiguration<Person>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.BaseConfig();

        builder.Property(e => e.Name)
            .HasMaxLength(250)
            .IsRequired();
    }
}
