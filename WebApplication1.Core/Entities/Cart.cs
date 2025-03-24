using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Core.Entities
{
    public class Cart : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
