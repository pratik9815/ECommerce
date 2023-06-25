using DataAccessLayer.Common.Category;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        IQueryable<GetCategoryCommand> GetAllCategory();
        Task<ApiResponse> CreateCategory(CreateCategoryCommand category);
        Task<ApiResponse> UpdateCategory(UpdateCategoryCommand category);
        Task<ApiResponse> RemoveCategory(Guid Id);
        Task<ApiResponse> CreateCategoryWithSubCategory(CreateCategoryWithSubCategoryCommand category);
        IQueryable<GetCategoryWithSubCategory> GetCategoryWithSubCategory();
        Task<ApiResponse> UpdateCategoryWithSubCategory(UpdateCategoryWithSubCategoryCommand category);
    }
}
