using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Core.Entities;

namespace WebApplication1.Repositry.Data.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("NOW()"); 

            builder.HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId)
                .IsRequired();

            builder.HasMany(u => u.Tokens)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
