using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Core.Entities
{
    public enum LanguageCode
    {
        EN = 1,  // English
        FR = 2,  // French
        AR = 3   // Arabic
    }

    public class ProductTranslation : BaseEntity
    {
        public Guid ProductId { get; set; }
        public LanguageCode LanguageCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Product Product { get; set; }
    }
}
