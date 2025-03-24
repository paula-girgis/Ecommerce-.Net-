using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Repositries;
using WebApplication1.Repositry.Data;

namespace WebApplication1.Repositry.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(EcommerceContext context) : base(context)
        {

        }

       public async Task<int> UpdateProductById(Guid id,UpdateProductDto UpdatedProduct)
        {
            var product = await context.Products.FindAsync(id);
            if (product is null)
                return 0;

            product.Quantity = UpdatedProduct.Quantity;
            product.Price = UpdatedProduct.Price;
            return await context.SaveChangesAsync();
        }

        
    }
    
}
