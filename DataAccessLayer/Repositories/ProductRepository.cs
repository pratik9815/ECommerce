using DataAccessLayer.Common.Product;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using DataAccessLayer.Query.Product;
using DataAccessLayer.Repositories.IRepositories;
using DataAccessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
        public IQueryable<GetProductQuery> GetAllProducts()
        {
                var products = _context.Products
                .OrderByDescending(o => o.CreatedDate)
                .Where(a => a.IsDeleted == false)
                .Select(p => new GetProductQuery
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
                    CategoryLists = p.ProductCategories.Select(x => new CategoryList
                    {
                        CategoryId = x.CategoryId,
                        CategoryName = x.Category.CategoryName
                    }).ToList(),
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
        public async Task<List<GetProductQuery>> GetWithImage()
         {
            var products = await _context.Products.Select(x => new GetProductQuery
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Quantity = x.Quantity,
                Img = x.Img,
                ProductStatus = x.ProductStatus,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                CreatedBy = _context.Users.FirstOrDefault(a => a.Id == x.CreatedBy).FullName,
                ImageLists = x.ProductImages.Select(y => new ImageList
                {
                    ImageId = y.Id,
                    ImageName = y.ImageName

                }).ToList(),
            }).ToListAsync();

            foreach (var item in products)
            {
                foreach (var img in item.ImageLists)
                {
                    if (img.ImageName is not null)
                    {
                        string path = _webHostEnvironment.WebRootPath + @"/images/" + img.ImageName;
                        using var image = Image.Load(path);
                        using var m = new MemoryStream();
                        image.Save(m, new JpegEncoder());
                        byte[] imageBytes = m.ToArray();
                        img.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(imageBytes);
                        break;
                    }

                }
            }


            return products;
        }   

        public async Task<ApiResponse> CreateProduct(CreateProductCommand product)
        {
            try
            {

                var newProduct = new Product
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quantity = product.Quantity
                };
                newProduct.AddProductCategory(newProduct, product.CategoryId);
                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();
                return new ApiResponse
                {
                    ResponseCode = 200,
                    Message = "Success"
                };
            }
            catch(Exception ex) 
            {
                var errors = new List<string>();
                errors.Add(ex.Message);
                return new ApiResponse
                {
                    ResponseCode = 400,
                    Message = "Failed",
                    Errors = errors
                };
            }

        }
        //public async Task<string> CreateProduct(CreateProductWithImageCommand product)
        //{
        //    try
        //    {
        //        var newProduct = new Product();
        //        newProduct.Name = product.Name;
        //        var a = _webHostEnvironment.WebRootPath;
        //        var fileWitPath = Path.Combine(a, "Images", newProduct.Name);
        //        if (!Directory.Exists(fileWitPath))
        //        {
        //            Directory.CreateDirectory(fileWitPath);
        //        }
        //        var ext = Path.GetExtension(product.Img.FileName);
        //        var allowedExtension = new string[] { ".jpg", ".png", ".jpeg", ".pdf" };

        //        if (!allowedExtension.Contains(ext))
        //        {
        //            string msg = string.Format("Only {0} extension are allowed", string.Join(",", allowedExtension));
        //            return "Not allowed format";
        //        }
        //        var fileName = Guid.NewGuid().ToString()  + ext;
        //        var filePath = Path.Combine(fileWitPath, fileName);

        //        using (var fileSteam = new FileStream(filePath, FileMode.Create))
        //        {
        //            await product.Img.CopyToAsync(fileSteam);
        //        }

        //        newProduct.Description = product.Description;
        //        newProduct.Price = product.Price;
        //        newProduct.Quantity = product.Quantity;
        //        newProduct.Img = fileName;
        //        _context.Products.Add(newProduct);
        //        await _context.SaveChangesAsync();
        //        return "Product created succesfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }

        //}
        public async Task<ApiResponse> UpdateProduct(UpdateProductCommand product)
        {
            try
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
                return new ApiResponse
                {
                    ResponseCode = 200,
                    Message = "Success"
                };
            }
            catch(Exception ex)
            {
                var errors = new List<string>();
                errors.Add(ex.Message);
                return new ApiResponse {
                    ResponseCode = 400,
                    Message = "Failed",
                    Errors = errors
                };
            }


        }

        public async Task<ApiResponse> RemoveProduct(Guid Id)
        {
            try
            {
                var delProduct = await _context.Products.FindAsync(Id);
                delProduct.IsDeleted = true;
                delProduct.DeletedBy = _service?.UserId;
                delProduct.DeletedDate = DateTime.UtcNow.AddHours(5).AddMinutes(45);
                await _context.SaveChangesAsync();
                return new ApiResponse { ResponseCode = 200, Message = "Success" };
            }
            catch (Exception ex)
            {
                var errors = new List<string>();
                errors.Add(ex.Message);
                return new ApiResponse
                {
                    ResponseCode = 500,
                    Message = "Failed",
                    Errors = errors
                };
            }

        }

        public async Task<ApiResponse> CreateProductWithMultipleImages(CreateProductWithImagesCommand product)
        {
            try
            {
                var errors = new List<string>();

                if (product.Img != null)
                {
                    var webRootPath = _webHostEnvironment.WebRootPath;
                    var fileWithPath = Path.Combine(webRootPath, "Images");
                    if (!Directory.Exists(fileWithPath))
                    {
                        Directory.CreateDirectory(fileWithPath);
                    }
                    var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg", ".pdf", ".avif" };
                    Product newProduct = new Product();
                    newProduct.Name = product.Name;
                    newProduct.Price = product.Price;
                    newProduct.Quantity = product.Quantity;
                    newProduct.Description = product.Description;
                    foreach(var category in product.CategoryId)
                    {
                        newProduct.AddProductCategory(newProduct, category);
                    }
                    
                    _context.Add(newProduct);
                    await _context.SaveChangesAsync();
                    foreach (var image in product.Img)
                    {
                        var ext = Path.GetExtension(image.FileName);
                        if (!allowedExtensions.Contains(ext))
                        {
                            string msg = string.Format("Only {0} extension are allowed", string.Join(",", allowedExtensions));
                            errors.Add(msg);
                            return new ApiResponse
                            {
                                ResponseCode = 400,
                                Message = "Failed",
                                Errors = errors
                            };
                        }
                        var uniqueString = Guid.NewGuid().ToString();
                        var fileName = uniqueString + ext;
                        var filePath = Path.Combine(fileWithPath, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        var productImage = new ProductImage
                        {
                            ProductId = newProduct.Id,
                            ImageName = fileName
                        };
                        _context.Add(productImage);
                    }
                    await _context.SaveChangesAsync();
                    return new ApiResponse
                    {
                        ResponseCode = 200,
                        Message = "Success"
                    };
                }
                else
                {
                    errors.Add("Please select atleast one image");
                    return new ApiResponse
                    {
                        ResponseCode = 400,
                        Message = "Failed",
                        Errors = errors
                    };
                }
            }
            catch (Exception ex)
            {
                var errors = new List<string>();
                errors.Add(ex.Message);
                return new ApiResponse
                {
                    ResponseCode = 500,
                    Message = "Failed",
                    Errors = errors
                };
            }

        }

        public async Task<List<GetProductQuery>> GetAllWithImage()
        {
            var products = await _context.Products
                .OrderByDescending(o => o.CreatedDate)
                .Where(a => a.IsDeleted == false)
                .Select(x => new GetProductQuery
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Img = x.Img,
                    ProductStatus = x.ProductStatus,
                    CreatedDate = x.CreatedDate,
                    UpdatedBy = x.UpdatedBy,
                    CreatedBy = _context.Users.FirstOrDefault(a => a.Id == x.CreatedBy).FullName,
                    ImageLists = x.ProductImages.Select(y => new ImageList
                    {
                        ImageId = y.Id,
                        ImageName = y.ImageName

                    }).ToList(),
                    CategoryLists = x.ProductCategories.Select(p => new CategoryList
                    {
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.CategoryName
                    }).ToList(),

                }).ToListAsync();

            foreach (var item in products)
            {
                foreach (var img in item.ImageLists)
                {
                    if (img.ImageName is not null)
                    {
                        string path = _webHostEnvironment.WebRootPath + @"/images/" + img.ImageName;
                        using var image = Image.Load(path);
                        using var m = new MemoryStream();
                        image.Save(m, new JpegEncoder());
                        byte[] imageBytes = m.ToArray();
                        img.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(imageBytes);
                    }

                }
            }
            return products;
        }

      
    }
}
