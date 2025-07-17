using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TropicFeel.Domain.Entities;

namespace TropicFeel.Infrastructure.Data.Configurations;

public class SalesOrderLogConfiguration : IEntityTypeConfiguration<SalesOrderLog>
{
    public void Configure(EntityTypeBuilder<SalesOrderLog> builder)
    {
    }
}
