using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TropicFeel.Domain.Entities;

namespace TropicFeel.Infrastructure.Data.Configurations;

public class CustomFormConfiguration : IEntityTypeConfiguration<CustomForm>
{
    public void Configure(EntityTypeBuilder<CustomForm> builder)
    {
        builder.Property(t => t.Text)
            .HasMaxLength(100)
            .IsRequired();
    }
}
