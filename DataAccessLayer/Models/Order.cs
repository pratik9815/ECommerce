using DataAccessLayer.Common;
using DataAccessLayer.Models.Common;
using DataAccessLayer.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Order : AuditableEntity
    {
        public Order()
        {
            OrderDetails = new List<OrderDetails>();    
        }
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public string ShippingAddress { get; set; }
        public string OrdersAddress { get; set; }
        public string OrderEmail { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }

        //Customer and order relationship
        public ICollection<OrderDetails> OrderDetails { get; set; }
        public void AddOrder(Order order, Guid ProductId)
        {
            var newOrder = new OrderDetails
            {
                OrderId = order.Id,
                ProductId = ProductId,
                Price = order.Amount,
                Quantity = 1,
            };
            OrderDetails.Add(newOrder);
        }
    }
}
