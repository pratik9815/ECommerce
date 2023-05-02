using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ProductImage
    {
        public Guid Id { get; set; }

        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        public string ImageName { get; set; }
    }
}
