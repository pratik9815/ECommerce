using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public string Shipping_Address { get; set; }
        public string Orders_Address { get; set; }
        public string Order_Email { get; set; }
        public string Order_Date { get; set; }
        public string Order_Status { get; set; }

        public Guid CustomerId { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
