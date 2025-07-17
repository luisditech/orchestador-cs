using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TropicFeel.Domain.Entities;

namespace TropicFeel.Infrastructure.Data.Configurations;

public class CustbodyPwkTipoSoConfiguration : IEntityTypeConfiguration<CustbodyPwkTipoSo>
{
    public void Configure(EntityTypeBuilder<CustbodyPwkTipoSo> builder)
    {
        builder.Property(t => t.Text)
            .HasMaxLength(100)
            .IsRequired();
    }
}
