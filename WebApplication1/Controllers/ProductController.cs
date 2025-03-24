using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Api.DTOs;
using WebApplication1.Api.Errors;
using WebApplication1.Core.Entities;
using WebApplication1.Repositry.Data;
using WebApplication1.Core.Repositries;
using WebApplication1.Service.WebApplication1.Repositry.WebApplication1.Core.services;

namespace WebApplication1.Api.Controllers
{

    public class ProductController : ApiBaseController
    {
        private readonly AuthService _authService;
        private readonly EcommerceContext _dbContext;
        private readonly ILogger<ProductController> _logger;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IProductRepository productRepository1;

        public ProductController
            (
            EcommerceContext dbContext,
            AuthService authService,
            ILogger<ProductController> logger,
            IGenericRepository<Product> productRepository,
            IProductRepository productRepository1
            )
        {
            _authService = authService;
            _dbContext = dbContext;
            _logger = logger;
            _productRepository = productRepository;
            this.productRepository1 = productRepository1;
        }


        #region Add Product
        [HttpPost("Add Product")]
        public async Task<ActionResult> Add_Product([FromBody] AddProductDto request, [FromHeader] string tk)
        {
           
            var product = new Product()
            {
                Price = request.Price,
                Quantity = request.Quantity,
                CreatedAt = DateTime.UtcNow

            };
            if (product is null)
                return BadRequest("product is null");
            var isAdded = await _productRepository.AddAsync(product);
            if (isAdded == 0)
                return BadRequest("an error occured during save");
            return Ok("Product saved successfully");




        }
        #endregion


        #region GetAllProducts
        [HttpGet("Get_All_Product")]
        public async Task<ActionResult> Get_All_Product([FromHeader] string tk)
        {
        
            var result = await _productRepository.GetAllAsync();
            if (result == null)
                return NotFound(new ApiErrorResponse(404, "there is no product found"));
            return Ok(result);

        }
        #endregion

        #region GetProductById
        [HttpGet("{Id}")]
        public async Task<ActionResult> Get_Product_ById(Guid Id, [FromHeader] string tk)
        {
            
            var result = await _productRepository.GetByIdAsync(Id);
            if (result == null)
                return NotFound(new ApiErrorResponse(404, "there is no product found"));
            return Ok(result);

        }
        #endregion

        #region UpdateProductById
        [HttpPost("{Id}")]
        public async Task<IActionResult> UpdateProductById(Guid Id, [FromHeader] string tk, UpdateProductDto request)
        {
            
            var result = await productRepository1.UpdateProductById(Id, request);
            if (result == 0)
                return BadRequest(new ApiErrorResponse(405, "error during update"));
            return Ok("Product updated successfully");
        }

        #endregion


        #region DeleteProductById
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProductById(Guid Id, [FromHeader] string tk)
        {
             var product = await _dbContext.Products.FindAsync(Id);
            if (product is null)
                return NotFound(new ApiErrorResponse(404, "Product Not Found"));
            var isDeleted = await _productRepository.DeleteAsync(product);
            if (isDeleted == 0)
                return NotFound(new ApiErrorResponse(405, "something went wrong"));
            return Ok("Product Deleted Successfully");

            #endregion

        }

    }
}
