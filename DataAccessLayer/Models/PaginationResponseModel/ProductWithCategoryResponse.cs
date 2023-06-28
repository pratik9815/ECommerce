using DataAccessLayer.Common.Product;
using DataAccessLayer.Query.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.PaginationResponseModel
{
    public class ProductWithCategoryResponse
    {
        public List<GetProductWithCategory> Product { get; set; } = new List<GetProductWithCategory>();
        public int Pages { get; set; }
        public int TotalPage { get; set; }
    }
    public class ProductWithSubCategoryResponse
    {
        public List<GetProductWithSubCategory> Product { get; set; } = new List<GetProductWithSubCategory>();
        public int Pages { get; set; }
        public int TotalPage { get; set; }
    }
}
