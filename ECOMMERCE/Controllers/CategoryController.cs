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
        [AllowAnonymous]
        public async Task<ActionResult<List<GetCategoryCommand>>> GetCategories()
        {
            var category = await _categoryRepository.GetAllCategory().ToListAsync();  
            

            return Ok(category);
        }
        [HttpPost("create-category")]
        public async Task<ActionResult<ApiResponse>> CreateCategory([FromBody] CreateCategoryCommand category)
        {
            var response = await _categoryRepository.CreateCategory(category);
            if (response.ResponseCode is not 200)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpPut("update-category")]
        public async Task<ActionResult<ApiResponse>> UpdateCategory([FromBody] UpdateCategoryCommand category)
        {
            var response = await _categoryRepository.UpdateCategory(category);
            if (response.ResponseCode is not 200)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteProduct([FromRoute] Guid id)
        {
            var response = await _categoryRepository.RemoveCategory(id);
            if (response.ResponseCode is not 200)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
