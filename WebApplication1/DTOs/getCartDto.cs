namespace WebApplication1.Api.DTOs
{
    public class getCartDto
    {
        public Guid Id { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }
}
