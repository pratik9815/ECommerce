using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common.Product
{
    public class GetProductCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public IFormFile Image { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
    public class CreateProductCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public IFormFile Img { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
    public class UpdateProductCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
