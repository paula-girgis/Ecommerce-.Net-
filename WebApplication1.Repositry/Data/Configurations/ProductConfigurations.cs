using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Core.Entities;

namespace WebApplication1.Repositry.Data.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Quantity)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasMany(p => p.Translations)
                .WithOne(t => t.Product)
                .HasForeignKey(t => t.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
