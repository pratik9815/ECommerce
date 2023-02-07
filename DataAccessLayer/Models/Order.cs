using DataAccessLayer.Common;
using DataAccessLayer.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Order : AuditableEntity
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public string ShippingAddress { get; set; }
        public string OrdersAddress { get; set; }
        public string OrderEmail { get; set; }
        public string OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public Guid CustomerId { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
