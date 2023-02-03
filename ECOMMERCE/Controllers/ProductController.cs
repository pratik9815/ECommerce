using DataAccessLayer.Common.Product;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECOMMERCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            var products = await _context.Products
                .OrderByDescending(o => o.CreatedDate)
                .Select(p => new GetProductCommand
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CreatedDate = p.CreatedDate,
                    UpdatedBy = p.UpdatedBy,
                    CreatedBy = _context.Users.FirstOrDefault(a => a.Id == p.CreatedBy).FullName,
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
            };
            _context.Products.Add(newProduct);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("update-product")]
        public async Task<ActionResult> UpdateProduct([FromBody] UpdateProductCommand product)
        {
            var upProduct = await _context.Products.FindAsync(product.Id);
            if (upProduct == null) return BadRequest();

            upProduct.Name = product.Name;
            upProduct.Description = product.Description;
            upProduct.Price = product.Price;

            _context.Products.Update(upProduct);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("delete-product/{id}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] Guid id)
        {
            var delProduct = await _context.Products.FindAsync(id);
            if(delProduct == null) return BadRequest();

            _context.Products.Remove(delProduct);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
