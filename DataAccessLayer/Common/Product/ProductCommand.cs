using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common.Product
{
    public class GetProductCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
    public class CreateProductCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Img { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
