using DataAccessLayer.Common.Product;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using DataAccessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductRepository(ApplicationDbContext context, ICurrentUserService service, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _service = service;
            _webHostEnvironment = webHostEnvironment;
        }
        public  IQueryable<GetProductCommand> GetAllProducts()
        {
            var products =  _context.Products
                .OrderByDescending(o => o.CreatedDate)
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
                });
            return products;
        }

        public async Task<Product> GetById(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
                return null; 
            return product;
        }
        public async Task<string> CreateProduct(CreateProductCommand product)
        {
            try
            {
                var newProduct = new Product();
                newProduct.Name = product.Name;
                var a = _webHostEnvironment.WebRootPath;
                var fileWitPath = Path.Combine(a, "Images", newProduct.Name);
                if (!Directory.Exists(fileWitPath))
                {
                    Directory.CreateDirectory(fileWitPath);
                }
                var ext = Path.GetExtension(product.Img.FileName);
                var allowedExtension = new string[] { ".jpg", ".png", ".jpeg", ".pdf" };

                if (!allowedExtension.Contains(ext))
                {
                    string msg = string.Format("Only {0} extension are allowed", string.Join(",", allowedExtension));
                    return "Not allowed format";
                }
                var fileName = Guid.NewGuid().ToString()  + ext;
                var filePath = Path.Combine(fileWitPath, fileName);

                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await product.Img.CopyToAsync(fileSteam);
                }

                newProduct.Description = product.Description;
                newProduct.Price = product.Price;
                newProduct.Quantity = product.Quantity;
                newProduct.Img = fileName;
                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();
                return "Product created succesfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public async Task UpdateProduct(UpdateProductCommand product)
        {
 
                var productDetails = await _context.Products.FindAsync(product.Id);
                productDetails.Name = product.Name;
                productDetails.Description = product.Description;
                productDetails.Price = product.Price;
                productDetails.UpdatedBy = _service?.UserId;
                productDetails.UpdatedDate = DateTime.UtcNow;
                //productDetails.ProductStatus = product.ProductStatus;
                productDetails.Quantity = product.Quantity;
                _context.Products.Update(productDetails);
                await _context.SaveChangesAsync();
        }

        public async Task RemoveProduct(Guid Id)
        {
            try
            {
                var delProduct = await _context.Products.FindAsync(Id);
                delProduct.IsDeleted = true;
                delProduct.DeletedBy = _service?.UserId;
                delProduct.DeletedDate = DateTime.UtcNow.AddHours(5).AddMinutes(45); 
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                
            }
           
        }
    }
}
