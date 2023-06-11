using DataAccessLayer.Common.Product;
using DataAccessLayer.Query.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.PaginationResponseModel
{
    public class ProductResponse
    {
        public List<GetProductQuery> Product { get; set; } = new List<GetProductQuery>();
        public int Pages { get; set; }
        public int TotalPage { get; set; }
    }
}
