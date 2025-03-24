using WebApplication1.Core.Entities;

namespace WebApplication1.Api.DTOs
{
    public class AddCartItemDto
    {
 
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
