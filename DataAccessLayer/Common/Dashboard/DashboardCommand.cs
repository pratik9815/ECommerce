using DataAccessLayer.Query.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common.Dashboard
{
    public class DashboardCommand
    {
        public double Amount { get; set; }
        public int Quantity { get; set; }   
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
    }

    public class GetPopularProducts
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
    }

    public class GetPopularProductsWithImage
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public ImageList ImageList { get; set; }
    }

}
