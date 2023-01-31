using DataAccessLayer.Common.Category;
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
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext  context)
        {
            _context = context;
        }
        [HttpGet("get-category")]
        public async Task<ActionResult<List<GetCategoryCommand>>> GetCategories()
        {
            var category = await _context.Categories.Select(c => new Category
            {
                CategoryName = c.CategoryName,
                Description = c.Description,
            }).ToListAsync();

            return Ok(category);
        }
        [HttpPost("create-category")]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryCommand category)
        {
            var newCategory = new Category
            {
                CategoryName = category.CategoryName,
                Description = category.Description,
            };
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            return Ok(newCategory);
        }
        [HttpPut("update-category")]
        public async Task<ActionResult> UpdateCategory([FromBody] UpdateCategoryCommand category)
        {
            var getCategory = await _context.Categories.FindAsync(category.Id);
            if (getCategory == null) 
                return BadRequest();
            getCategory.CategoryName = category.CategoryName;
            getCategory.Description = category.Description;
            
            return Ok();

        }
    }
}
