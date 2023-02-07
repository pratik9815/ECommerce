using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECOMMERCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        public ProductCategoryController()
        {
            //[HttpPost("create-product-category")]
            //public async Task<IActionResult> CreateProductCategory([FromBody] CreateProductCategoryCommand pc)
            //{
            //    foreach (var items in pc.CategoryId)
            //    {
            //        var something = new ProductCategory
            //        {
            //            ProductId = pc.ProductId,
            //            CategoryId = Guid.Parse(items),
            //        };
            //        _context.ProductCategories.Add(something);
            //        await _context.SaveChangesAsync();
            //    }

            //    return Ok();
            //}
        }
    }
}
