
using DataAccessLayer.Common.Product;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using DataAccessLayer.Models.PaginationResponseModel;
using DataAccessLayer.Query.Product;
using DataAccessLayer.Repositories.IRepositories;
using DataAccessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace ECOMMERCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext context,
            IProductRepository productRepository,
            ICurrentUserService userService,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _productRepository = productRepository;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("get-product")]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetProductCommand>>> GetProducts()
        {
            var products = await _productRepository.GetAllProducts().ToListAsync();

            return Ok(products);
        }

        [HttpGet("getbyid/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<GetProductQuery>> GetProductById([FromRoute] Guid id)
        {
            var product = await _productRepository.GetById(id);
            return Ok(product);
        }

        [HttpGet("get-with-image")]
        [AllowAnonymous]
        public async Task<List<GetProductQuery>> GetWithImage()
        {
            var product = await _productRepository.GetWithImage();
            return product;
        }

        [HttpGet("get-images-with-all-images")]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetProductQuery>>> GetAllWithImage()
        {
            var products = await _productRepository.GetAllWithImage();
            return Ok(products);
        }
        [HttpPost("create-product")]
        public async Task<ActionResult<ApiResponse>> CreateProduct([FromBody] CreateProductCommand product)
        {
            var response = await _productRepository.CreateProduct(product);
            if (response.ResponseCode is not 200)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpPost("create-product-with-multiple-image")]
        public async Task<ActionResult<ApiResponse>> CreateProductWithImages([FromForm] CreateProductWithImagesCommand product)
        {
            var response = await _productRepository.CreateProductWithMultipleImages(product);
            if (response.ResponseCode is not 200)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPut("update-product")]
        public async Task<ActionResult<ApiResponse>> UpdateProduct([FromBody] UpdateProductCommand product)
        {
            var response = await _productRepository.UpdateProduct(product);
            if (response.ResponseCode is not 200)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPut("delete-product/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteProduct([FromRoute] Guid id)
        {
            var response = await _productRepository.RemoveProduct(id);
            if (response.ResponseCode is not 200)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("get-product-with-image")]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetProductCommand>>> Get()
        {
            var products = await _productRepository.GetWithImage();
            return Ok(products);
        }


        [HttpGet("get-product-category")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductWithCategoryResponse>> GetProductCategory([FromQuery] string categoryId, [FromQuery] int page)
        {
            if (string.IsNullOrEmpty(categoryId))
                return BadRequest();
            var products = await _productRepository.GetProductWithCategories(categoryId,page);

            return Ok(products);
        }
        [HttpGet("get-product-respective-category")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductWithCategoryResponse>> GetProductWithRespectiveCategories([FromQuery] string[] categoryId, [FromQuery] int page)
        {
            //if (string.IsNullOrEmpty(categoryId))
            //    return BadRequest();
            var products = await _productRepository.GetProductWithRespectiveCategories(categoryId, page);

            return Ok(products);
        }

        [HttpGet("get-product-with-pagination/{page}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductResponse>> GetProductWithPagination([FromRoute]int page)
        {
            var product =await _productRepository.GetProductWithPagination(page);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

    }
}
