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
            var category = await _context.Categories.Select(c => new GetCategoryCommand
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                Description = c.Description,
                CreatedBy = _context.Users.FirstOrDefault(a => a.Id == c.CreatedBy).FullName,
                UpdatedBy = c.UpdatedBy,
                CreatedDate = c.CreatedDate
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
            
            _context.Categories.Update(getCategory);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] Guid id)
        {
            var delCategory = await _context.Categories.FindAsync(id);
            if(delCategory == null) return BadRequest();
            _context.Categories.Remove(delCategory);
            await _context.SaveChangesAsync();
            return Ok();    
        }
    }
}
