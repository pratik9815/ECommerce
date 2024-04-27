using DataAccessLayer.Common.Category;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using DataAccessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _service;

        public CategoryRepository(ApplicationDbContext context, ICurrentUserService service)
        {
            _context = context;
            _service = service;
        }
        public IQueryable<GetCategoryCommand> GetAllCategory()
        {
            var category = _context.Categories.
                OrderByDescending(o => o.CreatedDate).
                Where(a => a.IsDeleted == false).
                Select(c => new GetCategoryCommand
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName,
                    Description = c.Description,
                    CreatedBy = _context.Users.FirstOrDefault(a => a.Id == c.CreatedBy).FullName,
                    UpdatedBy = c.UpdatedBy,
                    CreatedDate = c.CreatedDate
                });
            return category;
        }
        public async Task<ApiResponse> CreateCategory(CreateCategoryCommand category)
        {

            try
            {
                var oldCategory = _context.Categories
                                          .Where(c => c.IsDeleted == false)
                                          .FirstOrDefault(a => a.CategoryName == category.CategoryName);
                if (oldCategory != null)
                {
                    return new ApiResponse
                    {
                        ResponseCode = 400,
                        Message = "Category already exists",
                    };
                }
                var newCategory = new Category
                {
                    CategoryName = category.CategoryName,
                    Description = category.Description,
                };
                _context.Categories.Add(newCategory);
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
        public async Task<ApiResponse> UpdateCategory(UpdateCategoryCommand category)
        {
            try
            {
                var categoryDetails = await _context.Categories.FindAsync(category.Id);
                if (categoryDetails == null)
                {
                    return new ApiResponse
                    {
                        ResponseCode = 400,
                        Message = "Category not found"
                    };
                }
                categoryDetails.CategoryName = category.CategoryName;
                categoryDetails.Description = category.Description;
                _context.Categories.Update(categoryDetails);
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
        public async Task<ApiResponse> RemoveCategory(Guid Id)
        {
            try
            {
                var removeProduct = await _context.Categories.FindAsync(Id);
                if (removeProduct == null)
                {
                    return new ApiResponse
                    {
                        ResponseCode = 400,
                        Message = "Category not found"
                    };
                }
                removeProduct.IsDeleted = true;
                removeProduct.DeletedDate = DateTime.UtcNow;
                removeProduct.DeletedBy = _service?.UserId;
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


        public async Task<ApiResponse> CreateCategoryWithSubCategory(CreateCategoryWithSubCategoryCommand category)
        {
            try
             {
                var oldCategory = _context.Categories
                          .Where(c => c.IsDeleted == false)
                          .FirstOrDefault(a => a.CategoryName == category.CategoryName);
                if (oldCategory != null)
                {
                    return new ApiResponse
                    {
                        ResponseCode = 400,
                        Message = "Category already exists",
                    };
                }

                var newCategory = new Category  
                {
                    Id = Guid.NewGuid(),
                    CategoryName = category.CategoryName,
                    Description = category.Description,
                };


                foreach (var subCategory in category.subCategory)
                {
                    var newSubCategory = new SubCategory
                    {
                        Id = Guid.NewGuid(),
                        SubCategoryName = subCategory.SubCategoryName,
                        SubCategoryDescription = subCategory.SubCategoryDescription
                    };
                    newCategory.AddSubCategories(newSubCategory);
                }

                await _context.AddAsync(newCategory);
                await _context.SaveChangesAsync();
                return new ApiResponse()
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
                    ResponseCode = 500,
                    Message = "Failed",
                    Errors = errors
                };
            }
        }

        public IQueryable<GetCategoryWithSubCategory> GetCategoryWithSubCategory()
        {
            var category = _context.Categories.Where(x => !x.IsDeleted).
                                Select(x => new GetCategoryWithSubCategory
                                {
                                    id = x.Id,
                                    CategoryName = x.CategoryName,
                                    Description = x.Description, 
                                    CreatedBy = _context.Users.FirstOrDefault(a => a.Id == x.CreatedBy).FullName,
                                    subCategories = x.SubCategories.Select(s => new GetSubCategory
                                    {
                                        Id = s.Id,
                                        SubCategoryName = s.SubCategoryName,
                                        SubCategoryDescription = s.SubCategoryDescription,
                                        
                                    }).ToList(),
                                });
            return category;
        }

        public async Task<ApiResponse> UpdateCategoryWithSubCategory(UpdateCategoryWithSubCategoryCommand category)
        {
            try
            {
                var categoryDetails = await _context.Categories.FindAsync(category.Id);
                if (categoryDetails == null)
                {
                    return new ApiResponse
                    {
                        ResponseCode = 400,
                        Message = "Category not found"
                    };
                }
                categoryDetails.CategoryName = category.CategoryName;
                categoryDetails.Description = category.Description;

                var subCategories = await _context.SubCategories.Where(sc => sc.CategoryId == categoryDetails.Id).ToListAsync();
                foreach (var subCategory in subCategories)
                {
                    _context.SubCategories.Remove(subCategory);
                }
                foreach (var subCategory in category.subCategory)
                {
                    var newSubCategory = new SubCategory
                    {
                        SubCategoryName = subCategory.SubCategoryName,
                        SubCategoryDescription = subCategory.SubCategoryDescription
                    };
                    categoryDetails.AddSubCategories(newSubCategory);
                }
                categoryDetails.UpdatedDate = DateTime.UtcNow.AddHours(5).AddMinutes(45);
                _context.Categories.Update(categoryDetails);
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
    }
}
