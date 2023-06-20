using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common.Order
{
    public class DeleteOrderCommand
    {
        public int quantity { get; set; }
        public Guid ProductId { get; set; }
    }
}
