using DataAccessLayer.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Product : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public double Price { get; set; }
        //public int Quantity { get; set; }
        public string Img { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
