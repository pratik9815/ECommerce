using DataAccessLayer.Common.Category;
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
        Task CreateCategory(CreateCategoryCommand category);
        Task UpdateCategory(UpdateCategoryCommand category);
        Task RemoveCategory(Guid Id);
    }
}
