namespace WebApplication1.Api.DTOs
{
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

}
