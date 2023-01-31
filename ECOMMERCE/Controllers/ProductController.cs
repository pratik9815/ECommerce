using DataAccessLayer.Common.Product;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECOMMERCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-product")]
        public async Task<ActionResult<List<GetProductCommand>>> GetProducts()
        {
            var products = await _context.Products.Select(p => new Product
            {
                Name = p.Name,
                Description= p.Description,
                Price= p.Price,
                CreatedDate= p.CreatedDate,
                CreatedBy= p.CreatedBy,

            }).ToListAsync();

            return Ok(products);
        }

        [HttpPost("create-product")]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductCommand product)
        {
            var newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Img = product.Img
            };
            _context.Products.Add(newProduct);


            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
