using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Api.DTOs;
using WebApplication1.Api.Errors;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Repositries;
using WebApplication1.Repositry.Data;

namespace WebApplication1.Api.Controllers
{
    
    public class ProductTranslationsController : ApiBaseController

        
    {
        private readonly IGenericRepository<Product> productRepo;
        private readonly EcommerceContext context;
        private readonly IGenericRepository<ProductTranslation> productTranslationRepo;
        

        public ProductTranslationsController(IGenericRepository<Product> ProductRepo, EcommerceContext context, IGenericRepository<ProductTranslation> productTranslationRepo)
        {
            productRepo = ProductRepo;
            this.context = context;
            this.productTranslationRepo = productTranslationRepo;
        }

        #region Add Translation
        [HttpPost("{productId}/Translation")]
        public async Task<IActionResult> AddProductTranslation([FromBody] AddProductTranslationRequestDto request ,Guid productId)
        {
            if (productId == Guid.Empty)
                return BadRequest("product not found");
            var product = await productRepo.GetByIdAsync(productId);
            if(product is null )
                return BadRequest("product not found");
            var translation = new ProductTranslation()
            {
                Description = request.Description,
                Name = request.Name,
                LanguageCode = request.LanguageCode,
                ProductId = productId,

            };

            await productTranslationRepo.AddAsync(translation);
            return Ok("Translation Added Successfully");



        }
        #endregion

        #region Get_Translation_PID
        [HttpGet ("{id}")]
        public async Task <ActionResult> Get_Translation_PID(Guid id , [FromHeader] string tk)
        {

            var result = await context.ProductTranslations
                          .Where(p => p.ProductId == id) 
                          .ToListAsync();
            if (!result.Any())
                return NotFound(new ApiErrorResponse (404 , $"there is no product translation to that product {id}"));
            return Ok(result);
        }
        #endregion
    }
}
