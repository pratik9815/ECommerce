using DataAccessLayer.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class OrderDetails : AuditableEntity
    {
        public Guid Id { get; set; }
        public double Price { get; set; } 
        public int Quantity { get; set; }
        //for order relationship
        public Order Order { get; set; }
        public Guid OrderId { get; set; }
        //for product realtionship
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
      
    }
}
