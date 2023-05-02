using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common.Order
{
    public class CreateOrderCommand
    {
        public double Amount { get; set; }
        public string ShippingAddress { get; set; }
        public string OrdersAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string OrderEmail { get; set; }     
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime OrderDate { get; set; }
    }
    public class GetOrderCommand
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public string ShippingAddress { get; set; }
        public string OrdersAddress { get; set; }
        public string OrderEmail { get; set; }
        public string CustomerId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime OrderDate { get; set; }
    }

}
