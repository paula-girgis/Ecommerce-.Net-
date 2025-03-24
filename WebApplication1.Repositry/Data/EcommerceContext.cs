using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Core.Entities;

namespace WebApplication1.Repositry.Data
{
    public class EcommerceContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>

    {
        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Token> Tokens { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
    }
}
