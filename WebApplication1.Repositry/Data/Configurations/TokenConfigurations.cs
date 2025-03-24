using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Core.Entities;

namespace WebApplication1.Repositry.Data.Configurations
{
    public class TokenConfigurations : IEntityTypeConfiguration<Token>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Token> T)
        {
            T.HasKey(T => T.Id);

            T.Property(T => T.isValid)
             .HasDefaultValue(false);

            T.HasOne(t => t.User)
                .WithMany(u => u.Tokens)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
