using DataAccessLayer.Common.Product;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IProductRepository
    {
        IQueryable<GetProductCommand> GetAllProducts();
        Task<Product> GetById(Guid id);
        Task<string> CreateProduct(CreateProductCommand product);
        Task UpdateProduct(UpdateProductCommand product);
        Task RemoveProduct(Guid Id);    
    }
}
