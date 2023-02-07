using DataAccessLayer.Common.Product;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
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
        public async Task<ActionResult<List<GetProductCommand>>> GetProducts()
        {
            var products = await _productRepository.GetAllProducts().ToListAsync();

            return Ok(products);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<GetProductCommand>> GetProductById([FromRoute] Guid id)
        {
            var product = await _productRepository.GetById(id);
            return Ok(product);
        }


        [HttpPost("create-product")]
        public async Task<ActionResult> CreateProduct([FromForm] CreateProductCommand product)
        {
            await _productRepository.CreateProduct(product);
            return Ok();  
        }

        [HttpPut("update-product")]
        public async Task<ActionResult> UpdateProduct([FromBody] UpdateProductCommand product)
        {
            await _productRepository.UpdateProduct(product);
            return Ok();
        }

        [HttpPut("delete-product/{id}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] Guid id)
        {
            await _productRepository.RemoveProduct(id); 
            return Ok();
        }

        [HttpGet("get-product-with-image")]
        public async Task<ActionResult<List<GetProductCommand>>> Get()
        {

            var products = await _context.Products
                                .OrderByDescending(x => x.CreatedDate)
                                .Where(a => a.IsDeleted == false)
                                .Select(p => new GetProductCommand
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Description = p.Description,
                                    Price = p.Price,
                                    Quantity = p.Quantity,
                                    Img = p.Img,
                                    ProductStatus = p.ProductStatus,
                                    CreatedDate = p.CreatedDate,
                                    UpdatedBy = p.UpdatedBy,
                                    CreatedBy = _context.Users.FirstOrDefault(a => a.Id == p.CreatedBy).FullName,
                                }).ToListAsync();


            foreach (var item in products)
            {
                if (item.Img is not null)
                {
                    string path = _webHostEnvironment.WebRootPath + @"/images/" + item.Name + @"/" + item.Img;
                    using var image = Image.Load(path);
                    using var m = new MemoryStream();
                    image.Save(m, new JpegEncoder());
                    byte[] imageBytes = m.ToArray();
                    item.Img = "data:image/jpeg;base64," + Convert.ToBase64String(imageBytes);
                }
            }

            //if (products != null)
            //{
            //    products.ForEach(item =>
            //    {
            //        item.Img = GetImage(item.Img, item.Name);
            //    });
            //}
            //else
            //{
            //    return NotFound();
            //}
            return Ok(products);

        }


        //private string GetFilePath(string name, string itemName)
        //{
        //    var fileWitPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images",itemName);
        //    return fileWitPath + "\\" + name;
        //}

        //private string GetImage(string name,string itemName)
        //{

        //    string imageUrl = string.Empty;
        //    //we can directly get the host url using
        //    //var HostUrl = HttpContext.Request.Host;
        //    string HostUrl = "https://localhost:7069";
        //    string filePath = GetFilePath(name,itemName);

        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        return "opps";
        //    }
        //    else
        //    {
        //        imageUrl = HostUrl + "/images/" + itemName + "/" + name;
        //    }
        //    return imageUrl;
        //}
    }
}
