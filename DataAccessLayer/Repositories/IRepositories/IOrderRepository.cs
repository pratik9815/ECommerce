using DataAccessLayer.Common;
using DataAccessLayer.Common.Order;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        IQueryable<GetOrderCommand> GetAllOrders();
        Task<ApiResponse> CreateOrder(CreateOrderCommand order);
        IQueryable<GetOrderCommand> GetProductWithCustomerId(Guid customerId, int page, OrderStatus orderStatus);
        Task<int> RemoveOrder(Guid orderId);
        Task<List<GetOrderCommand>> GetOrderWithStatus(OrderStatus orderStatus);
        Task<int> ChangeOrderStatus(string orderId, OrderStatus orderStatus);
    }
}
