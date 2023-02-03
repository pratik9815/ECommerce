using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class OrderDetails
    {
        public Guid Id { get; set; }
        public double Price { get; set; } 
        public int Quantity { get; set; }

        public Order Order { get; set; }
        public Guid OrderId { get; set; }

        public Product Product { get; set; }
        public Guid ProductId { get; set; }
      
    }
}
