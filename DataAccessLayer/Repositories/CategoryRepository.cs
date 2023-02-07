using DataAccessLayer.Common.Category;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using DataAccessLayer.Services.Interfaces;
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
        public async Task CreateCategory(CreateCategoryCommand category)
        {
            var newCategory = new Category
            {
                CategoryName = category.CategoryName,
                Description = category.Description,
            };
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCategory(UpdateCategoryCommand category)
        {
            var categoryDetails = await _context.Categories.FindAsync(category.Id);
            if (categoryDetails == null) { return; }
            categoryDetails.CategoryName = category.CategoryName;
            categoryDetails.Description = category.Description;
            _context.Categories.Update(categoryDetails);
            await _context.SaveChangesAsync();
        } 
        public async Task RemoveCategory(Guid Id)
        {
            var removeProduct = await _context.Products.FindAsync(Id);

            removeProduct.IsDeleted = true;
            removeProduct.DeletedDate = DateTime.UtcNow;
            removeProduct.DeletedBy = _service?.UserId;
            await _context.SaveChangesAsync();
        }
    }
}
