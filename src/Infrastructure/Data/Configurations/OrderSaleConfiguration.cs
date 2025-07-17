using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TropicFeel.Domain.Entities;

namespace TropicFeel.Infrastructure.Data.Configurations;
public class OrderSaleConfiguration : IEntityTypeConfiguration<SalesOrder>
{
    public void Configure(EntityTypeBuilder<SalesOrder> builder)
    {
        builder.Property(t => t.OrderReferenceJlp)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.HasMany(i => i.Logs)
            .WithOne(o => o.SalesOrder)
            .HasForeignKey(i => i.SalesOrderId);
    }
}
