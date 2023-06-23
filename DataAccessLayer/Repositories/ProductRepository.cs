using DataAccessLayer.Common.Dashboard;
using DataAccessLayer.Common.Product;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using DataAccessLayer.Models.PaginationResponseModel;
using DataAccessLayer.Query.Product;
using DataAccessLayer.Repositories.IRepositories;
using DataAccessLayer.Services;
using DataAccessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductRepository(ApplicationDbContext context, ICurrentUserService service,
            IWebHostEnvironment webHostEnvironment)
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
                //Img = p.Img,
                ProductStatus = p.ProductStatus,
                CreatedDate = p.CreatedDate,
                UpdatedBy = p.UpdatedBy,
                CreatedBy = _context.Users.FirstOrDefault(a => a.Id == p.CreatedBy).FullName,
                //Categories = p.ProductCategories.Select(x => x.CategoryId).ToList(),
                //This will only return the id and the name , we created a seprate class to return in the getproductquery class which is later modified
            });
            return products;



        }

        public async Task<GetProductQuery> GetById(Guid id)
        {
            var product = await _context.Products.Where(p => (p.Id == id && p.IsDeleted == false))
                                          .Select(x => new GetProductQuery
                                          {
                                              Id = x.Id,
                                              Name = x.Name,
                                              Description = x.Description,
                                              Price = x.Price,
                                              Quantity = x.Quantity,
                                              ProductStatus = x.ProductStatus,
                                              CreatedDate = x.CreatedDate,
                                              UpdatedBy = _context.Users.FirstOrDefault(y => y.Id == x.UpdatedBy).FullName,

                                              CreatedBy = _context.Users.FirstOrDefault(a => a.Id == x.CreatedBy).FullName,
                                              ImageLists = x.ProductImages.Select(y => new ImageList
                                              {
                                                  ImageId = y.Id,
                                                  ImageName = y.ImageName

                                              }).ToList(),

                                              Categories = x.ProductCategories.Select(x => new CategoryList
                                              {
                                                  CategoryId = x.CategoryId,
                                                  CategoryName = x.Category.CategoryName
                                              }).ToList(),
                                          }).FirstAsync();

            foreach (var img in product.ImageLists)
            {
                if (img.ImageName is not null)
                {
                    string path = _webHostEnvironment.WebRootPath + @"/images/" + img.ImageName;
                    img.ImageUrl = ImageService.GetByteImage(img, path);
                }

            }


            if (product is null)
                return null;
            return product;
        }
        public IQueryable<GetProductQuery> GetWithImage()
        {
            var products = _context.Products
                 .OrderByDescending(x => x.CreatedDate)
                 .Where(x => x.IsDeleted == false).AsNoTracking()
                 .Select(x => new GetProductQuery
                 {
                     Id = x.Id,
                     Name = x.Name,
                     Description = x.Description,
                     Price = x.Price,
                     Quantity = x.Quantity,
                     ProductStatus = x.ProductStatus,
                     CreatedDate = x.CreatedDate,
                     UpdatedBy = _context.Users.FirstOrDefault(y => y.Id == x.UpdatedBy).FullName,
                     CreatedBy = _context.Users.FirstOrDefault(a => a.Id == x.CreatedBy).FullName,
                     Categories = x.ProductCategories.Select(x => new CategoryList
                     {
                         CategoryId = x.CategoryId,
                         CategoryName = x.Category.CategoryName
                     }).ToList(),
                     ImageLists = x.ProductImages.Select(y => new ImageList
                     {
                         ImageName = y.ImageName
                     }).ToList(),

                 });

            foreach (var item in products)
            {
                foreach (var img in item.ImageLists)
                {
                    if (img.ImageName is not null)
                    {
                        string path = _webHostEnvironment.WebRootPath + @"/images/" + img.ImageName;
                        img.ImageUrl = ImageService.GetByteImage(img, path);
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
                var category = new ProductCategory
                {
                    CategoryId = product.CategoryId
                };
                newProduct.AddProductCategory(category);

                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();
                return new ApiResponse
                {
                    ResponseCode = 200,
                    Message = "Success"
                };
            }
            catch (Exception ex)
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

        //This is used in angular
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
                productDetails.ProductStatus = product.ProductStatus;

                var productCategories = await _context.ProductCategories.Where(c => c.ProductId == product.Id).ToListAsync();
                foreach (var category in productCategories)
                {
                    _context.ProductCategories.Remove(category);
                }

                foreach (var category in product.Categories)
                {
                    var productCategory = new ProductCategory
                    {
                        CategoryId = category
                    };
                    productDetails.AddProductCategory(productCategory);
                }

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
            catch (Exception ex)
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

        //This is used in angualr
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

        //This is used in angular
        public async Task<ApiResponse> CreateProductWithMultipleImages(CreateProductWithImagesCommand product)
        {
            try
            {
                List<string> errors = new List<string>();

                if (product.Img != null)
                {
                    Product newProduct = new Product();
                    newProduct.Name = product.Name;
                    newProduct.Price = product.Price;
                    newProduct.Quantity = product.Quantity;
                    newProduct.Description = product.Description;
                    newProduct.ProductStatus = product.ProductStatus;
                    foreach (var category in product.CategoryId)
                    {
                        var productCategory = new ProductCategory
                        {
                            CategoryId = category
                        };
                        newProduct.AddProductCategory(productCategory);
                    }
                    //var webRootPath = _webHostEnvironment.WebRootPath;
                    string fileWithPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

                    if (!Directory.Exists(fileWithPath))
                    {
                        Directory.CreateDirectory(fileWithPath);
                    }
                    string[] allowedExtensions = new string[] { ".jpg", ".png", ".jpeg", ".pdf", ".avif" };

                    foreach (var image in product.Img)
                    {
                        var productImage = await ImageService.StoreImage(image, fileWithPath);
                        _context.ProductImages.Add(productImage);
                        newProduct.AddProductImages(productImage);
                    }

                    _context.Add(newProduct);
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

        //This is used in angular
        public async Task<List<GetProductQuery>> GetAllWithImage()
        {
            var products = await _context.Products
                .OrderByDescending(o => o.CreatedDate)
                .Where(a => a.IsDeleted == false).AsNoTracking()
                .Select(x => new GetProductQuery
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    ProductStatus = x.ProductStatus,
                    CreatedDate = x.CreatedDate,
                    UpdatedBy = _context.Users.FirstOrDefault(y => y.Id == x.UpdatedBy).FullName,
                    CreatedBy = _context.Users.FirstOrDefault(a => a.Id == x.CreatedBy).FullName,
                    ImageLists = x.ProductImages.Select(y => new ImageList
                    {
                        ImageId = y.Id,
                        ImageName = y.ImageName
                    }).ToList(),
                    Categories = x.ProductCategories.Select(x => new CategoryList
                    {
                        CategoryId = x.CategoryId,
                        CategoryName = x.Category.CategoryName
                    }).ToList(),
                }).ToListAsync();

            foreach (var item in products)
            {
                foreach (var img in item.ImageLists)
                {
                    if (img.ImageName is not null)
                    {
                        string path = _webHostEnvironment.WebRootPath + @"/images/" + img.ImageName;
                        img.ImageUrl = ImageService.GetByteImage(img, path);
                    }
                }
            }
            return products;
        }
        //Used in angular
        public async Task<ProductWithCategoryResponse> GetProductWithCategories(string categoryId, int page)
        {
            var pageResult = 3f;
            var totalCount = _context.ProductCategories
                .Count(x => x.CategoryId.ToString() == categoryId && !x.Product.IsDeleted);

            var totalPage = (int)Math.Ceiling(totalCount / pageResult);

            var productWithCategory = await _context.ProductCategories
                                                .OrderByDescending(o => o.CreatedDate)
                                                .Where(x => (x.CategoryId.ToString() == categoryId && !x.Product.IsDeleted)).AsNoTracking()
                                                .Skip((page - 1) * (int)pageResult).Take((int)pageResult)
                                                .Select(x => new GetProductWithCategory
                                                {
                                                    Id = x.ProductId,
                                                    Name = x.Product.Name,
                                                    Quantity = x.Product.Quantity,
                                                    Description = x.Product.Description,
                                                    Price = x.Product.Price,
                                                    Img = x.Product.ProductImages.Select(i => new ImageList
                                                    {
                                                        ImageName = i.ImageName
                                                    }).FirstOrDefault(),
                                                    CategoryName = x.Category.CategoryName,
                                                }).ToListAsync();

            if (productWithCategory != null)
            {
                foreach (var product in productWithCategory)
                {
                    if (product.Img is not null)
                    {
                        string path = _webHostEnvironment.WebRootPath + @"/images/" + product.Img.ImageName;
                        product.Img.ImageUrl = ImageService.GetByteImage(product.Img, path);
                    }
                }
            }
            var productWithPagination = new ProductWithCategoryResponse
            {
                Product = productWithCategory,
                Pages = page,
                TotalPage = totalPage
            };
            return productWithPagination;
        }

        //Used in angular
        public async Task<ProductWithCategoryResponse> GetProductWithRespectiveCategories(string[] categoryId, int page)
        {
            var pageResult = 3f;
            //var totalCount = _context.ProductCategories
            //    .Count(x => categoryId.Contains(x.CategoryId.ToString()) && !x.Product.IsDeleted);


            var totalCount = _context.ProductCategories
                                        .Where(x => categoryId.Contains(x.CategoryId.ToString()) && !x.Product.IsDeleted)
                                        .Select(x => x.ProductId)
                                        .Distinct()
                                        .Count();
            var totalPage = (int)Math.Ceiling(totalCount / pageResult);





            var productWithCategory = await _context.ProductCategories

                                                                            .Where(x => categoryId.Contains(x.CategoryId.ToString()) && !x.Product.IsDeleted)
                                                                            .OrderByDescending(o => o.CreatedDate)
                                                                            .Select(x => new GetProductWithCategory
                                                                            {
                                                                                Id = x.ProductId,
                                                                                Name = x.Product.Name,
                                                                                Quantity = x.Product.Quantity,
                                                                                Description = x.Product.Description,
                                                                                Price = x.Product.Price,
                                                                                Img = x.Product.ProductImages.Select(i => new ImageList
                                                                                {
                                                                                    ImageName = i.ImageName
                                                                                }).FirstOrDefault(),
                                                                                CategoryName = x.Category.CategoryName,
                                                                            }).ToListAsync();

            var distinctProductsWithCategory = productWithCategory
                .GroupBy(x => x.Id)
                .Select(g => g.FirstOrDefault())
                .Skip((page - 1) * (int)pageResult)
                .Take((int)pageResult)
                .ToList();

            if (productWithCategory != null)
            {
                foreach (var product in productWithCategory)
                {
                    if (product.Img is not null)
                    {
                        string path = _webHostEnvironment.WebRootPath + @"/images/" + product.Img.ImageName;
                        product.Img.ImageUrl = ImageService.GetByteImage(product.Img, path);
                    }
                }
            }

            var productWithPagination = new ProductWithCategoryResponse
            {
                Product = distinctProductsWithCategory,
                Pages = page,
                TotalPage = totalPage
            };

            return productWithPagination;
        }

        //Used in angular
        public async Task<ProductResponse> GetProductWithPagination(int page)
        {
            var pageResult = 6f;
            var totalCount = _context.Products.Where(x => x.IsDeleted == false).Count();

            var pageCount = Math.Ceiling(_context.Products.Where(x => x.IsDeleted == false).Count() / pageResult);

            //Skip will skip the first (page-1)*()intpageResult
            //Take will take the first 3 product after skipping 
            var products = await _context.Products

                .Where(a => a.IsDeleted == false)
                .OrderByDescending(o => o.CreatedDate).AsNoTracking()
                .Skip((page - 1) * (int)pageResult).Take((int)pageResult)
                .Select(x => new GetProductListCommand
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    ProductStatus = x.ProductStatus,
                    ImageLists = x.ProductImages.Select(y => new ImageList
                    {
                        ImageId = y.Id,
                        ImageName = y.ImageName
                    }).FirstOrDefault(),
                }).ToListAsync();


            if (products != null)
            {
                foreach (var product in products)
                {
                    if (product.ImageLists is not null)
                    {
                        string path = _webHostEnvironment.WebRootPath + @"/images/" + product.ImageLists.ImageName;
                        product.ImageLists.ImageUrl = ImageService.GetByteImage(product.ImageLists, path);
                    }
                }
            }


            var productWithPagination = new ProductResponse
            {
                Product = products,
                Pages = page,
                TotalPage = (int)pageCount
            };
            return productWithPagination;
        }

        public async Task<List<GetProductListCommand>> GetLimitedProducts()
        {
            var products = await _context.Products
                .Where(x => !x.IsDeleted)
                .OrderBy(r => Guid.NewGuid())
                .AsNoTracking()
                .Take(3).
                Select(x => new GetProductListCommand
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    ProductStatus = x.ProductStatus,
                    ImageLists = x.ProductImages.Select(y => new ImageList
                    {
                        ImageId = y.Id,
                        ImageName = y.ImageName
                    }).FirstOrDefault(),
                }).ToListAsync();



            if (products != null)
            {
                foreach (var product in products)
                {
                    if (product.ImageLists is not null)
                    {
                        string path = _webHostEnvironment.WebRootPath + @"/images/" + product.ImageLists.ImageName;
                        product.ImageLists.ImageUrl = ImageService.GetByteImage(product.ImageLists, path);
                    }
                }
            }
            return products;
        }

        public IQueryable<GetPopularProductsWithImage> GetPopularProduct()
        {
            var products = _context.OrderDetails
                                                            .Where(o => !o.IsDeleted && !o.Order.IsDeleted)
                                                            .GroupBy(od => new { od.Product.Name, od.ProductId, od.Product.Price })
                                                            .Select(x => new GetPopularProductsWithImage
                                                            {
                                                                Quantity = x.Sum(od => od.Quantity),
                                                                TotalPrice = x.Key.Price,
                                                                ProductId = x.Key.ProductId,
                                                                ProductName = x.Key.Name,
                                                            }).OrderByDescending(o => o.Quantity).Take(3).ToList();

            if (products != null)
            {
                foreach (var product in products)
                {
                    var image = _context.ProductImages.FirstOrDefault(y => y.ProductId == product.ProductId);
                    if (image != null)
                    {
                        string path = _webHostEnvironment.WebRootPath + @"/images/" + image.ImageName;
                        product.ImageList = new ImageList
                        {
                            ImageId = image.Id,
                            ImageName = image.ImageName,
                        };
                        product.ImageList.ImageUrl = ImageService.GetByteImage(product.ImageList, path);
                    }

                }
            }

            return products.AsQueryable();
        }

    }
}
