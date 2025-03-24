using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Api.DTOs;
using WebApplication1.Api.Errors;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Repositries;
using WebApplication1.Repositry.Data;

namespace WebApplication1.Api.Controllers
{

    public class CartController : ApiBaseController
    {
        private readonly IGenericRepository<Cart> cartRepo;
        private readonly EcommerceContext context;

        public CartController(IGenericRepository<Cart> CartRepo, EcommerceContext context) {
            cartRepo = CartRepo;
            this.context = context;
        }

        #region add items to cart
        [HttpPost("{UserId}/items")]
        public async Task<IActionResult> AddProductToCart([FromHeader] string tk, [FromBody] AddCartItemDto model , Guid UserId)
        {

          
            
            // Get user's cart
            var cart = await context.Carts.Include(c => c.CartItems)
                                           .FirstOrDefaultAsync(c => c.UserId == UserId);
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == model.ProductId);

            // Create cart if it doesn't exist
            if (cart == null)
            {
                cart = new Cart()
                {
                    UserId = UserId,
                    CreatedAt = DateTime.UtcNow,
                    CartItems = new List<CartItem>()
                };

                await context.Carts.AddAsync(cart);
                await context.SaveChangesAsync();
            }

            // Check if the product is already in the cart
            var cartItem = await context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ProductId == model.ProductId);

            if (cartItem != null)
            {
                if (model.Quantity > product.Quantity)
                    return BadRequest(new ApiErrorResponse(405, "not enough elements"));
                // Product already exists, update quantity
                cartItem.Quantity += model.Quantity;
                product.Quantity -= model.Quantity;
                await context.SaveChangesAsync();
            }

            else
            {
                // Product does not exist, add new entry
                cartItem = new CartItem()
                {
                    CartId = cart.Id,
                    ProductId = model.ProductId,
                    Quantity = model.Quantity
                };

                await context.CartItems.AddAsync(cartItem);
            }

            // Save changes after updating/inserting the cart item
            await context.SaveChangesAsync();

            return Ok("The product was added successfully");
        }

        #endregion

        #region Get cart details 
        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetCartDetails(Guid UserId, [FromHeader] string tk)
        {
           
            var cart = await context.Carts
        .Include(c => c.CartItems)
        .ThenInclude(ci => ci.Product)
        .ThenInclude(p => p.Translations) // Include translations
        .FirstOrDefaultAsync(c => c.UserId == UserId);
            if (cart == null)
                return BadRequest(new ApiErrorResponse(404, "CartItems not found"));
            var cartDto = new getCartDto
            {
                Id = cart.Id,
                CartItems = cart.CartItems.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    ProductName = ci.Product.Translations.FirstOrDefault()?.Name,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity
                }).ToList()
            };
            return Ok(cartDto);
        }

        #endregion

        #region Remove a product from the cart
        [HttpDelete("{UserId}/Item/{ProductId}")]
        public async Task<IActionResult> Remove_Product(Guid UserId, Guid ProductId,  [FromHeader] string tk , [FromBody]  int Quantity) 
        {
          

            var cart = await context.Carts.FirstOrDefaultAsync(c => c.UserId == UserId);
            if (cart == null) return NotFound(new ApiErrorResponse(404 , "cart not found"));
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == ProductId);

            var cartitem = await context.CartItems.FirstOrDefaultAsync(ci=>ci.ProductId == ProductId && ci.CartId == cart.Id);
            if (cartitem == null) return NotFound(new ApiErrorResponse(404, "cart not found"));
            else
            {
                if (cartitem.Quantity >= Quantity)
                {
                    cartitem.Quantity -= Quantity;
                    product.Quantity += Quantity;
                    await context.SaveChangesAsync();
                }
                 
                else return BadRequest(new ApiErrorResponse(405 , " the quantity bigger than Quantity in your Cart"));
                if (cartitem.Quantity == 0)
                {
                    context.CartItems.Remove(cartitem);

                    await context.SaveChangesAsync();
                }

                }
           
            return Ok($"the product deleted successfully");
        }
        #endregion
    }
}
