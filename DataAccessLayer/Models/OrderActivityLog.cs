using DataAccessLayer.Common;
using DataAccessLayer.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class OrderActivityLog : AuditableEntity
    {
        public Guid Id { get; set; }
        public Order Order { get; set; }    
        public Guid OrderId { get; set; }   
        public OrderStatus OrderStatus { get; set; }
    }
}
