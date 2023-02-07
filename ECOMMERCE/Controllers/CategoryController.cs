using DataAccessLayer.Common.Category;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
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
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ApplicationDbContext  context, ICategoryRepository categoryRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
        }
        [HttpGet("get-category")]
        public async Task<ActionResult<List<GetCategoryCommand>>> GetCategories()
        {
            var category = await _categoryRepository.GetAllCategory().ToListAsync();  

            return Ok(category);
        }
        [HttpPost("create-category")]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryCommand category)
        {
            await _categoryRepository.CreateCategory(category);
            return Ok();
        }
        [HttpPut("update-category")]
        public async Task<ActionResult> UpdateCategory([FromBody] UpdateCategoryCommand category)
        {
            await _categoryRepository.UpdateCategory(category);
            return Ok();
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] Guid id)
        {
            await _categoryRepository.RemoveCategory(id);
            return Ok();    
        }
    }
}
