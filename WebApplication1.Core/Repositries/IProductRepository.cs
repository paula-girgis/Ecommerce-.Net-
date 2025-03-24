using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Core.Entities;
namespace WebApplication1.Core.Repositries
{
    public interface IProductRepository : IGenericRepository<Product>
    {
       
        Task<int> UpdateProductById(Guid id, UpdateProductDto UpdatedProduct);
    }
    public class UpdateProductDto
    {

        public int Price { get; set; }
        public int Quantity { get; set; }



    }
}
