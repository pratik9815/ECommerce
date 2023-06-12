using DataAccessLayer.Common;
using DataAccessLayer.Models.Common;

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
