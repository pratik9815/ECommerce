using DataAccessLayer.Common.Product;
using DataAccessLayer.Models;
using DataAccessLayer.Models.PaginationResponseModel;
using DataAccessLayer.Query.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IProductRepository
    {
        IQueryable<GetProductQuery> GetAllProducts();
        Task<List<GetProductQuery>> GetWithImage();
        Task<GetProductQuery> GetById(Guid id);
        //Task<string> CreateProduct(CreateProductWithImageCommand product);
        Task<ApiResponse> CreateProduct(CreateProductCommand product);
        Task<ApiResponse> UpdateProduct(UpdateProductCommand product);
        Task<ApiResponse> RemoveProduct(Guid Id);
        Task<ApiResponse> CreateProductWithMultipleImages(CreateProductWithImagesCommand product);
        Task<List<GetProductQuery>> GetAllWithImage();
        //Task<List<GetProductWithCategory>> GetProductWithCategories(string categoryId);
        Task<ProductWithCategoryResponse> GetProductWithCategories(string categoryId,int page);
        Task<ProductWithCategoryResponse> GetProductWithRespectiveCategories(string[] categoryId, int page);
        Task<ProductResponse> GetProductWithPagination(int page);
    }
}
