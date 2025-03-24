using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Core.Entities
{
    public class Product : BaseEntity
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ProductTranslation> ?Translations { get; set; }
    }

}
