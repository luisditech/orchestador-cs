using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TropicFeel.Domain.Entities;

namespace TropicFeel.Infrastructure.Data.Configurations;

public class ProductMappingConfiguration : IEntityTypeConfiguration<ProductMapping>
{
    public void Configure(EntityTypeBuilder<ProductMapping> builder)
    {
        builder.Property(t => t.JlpSku)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.SprintSku)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.NetSuiteSku)
            .IsRequired()
            .HasMaxLength(50);
    }
}
