using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TropicFeel.Domain.Entities;

namespace TropicFeel.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasMany(i => i.Items)
            .WithOne(o => o.Order)
            .HasForeignKey(i => i.OrderId);
    }
}
