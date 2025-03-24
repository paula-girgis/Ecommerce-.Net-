using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Core.Entities;

namespace WebApplication1.Repositry.Data.Configurations
{
    public class ProductTranslationConfigurations : IEntityTypeConfiguration<ProductTranslation>
    {
        public void Configure(EntityTypeBuilder<ProductTranslation> builder)
        {
            builder.HasKey(pt => pt.Id);

            builder.Property(pt => pt.LanguageCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(pt => pt.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(pt => pt.Description)
                .IsRequired()
                .HasMaxLength(1000);
        }
    }
}
