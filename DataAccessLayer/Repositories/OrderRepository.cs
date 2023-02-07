using DataAccessLayer.Common.Order;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<GetOrderCommand> GetAllOrders()
        {
            var orders = _context.Orders.
                            OrderByDescending(o=>o.OrderDate).
                            Where(o => o.IsDeleted == false).
                            Select(o => new GetOrderCommand
                            {
                                Id= o.Id,
                                OrdersAddress= o.OrdersAddress,
                                ShippingAddress= o.ShippingAddress,
                                Amount= o.Amount,
                                OrderEmail= o.OrderEmail,
                                OrderDate= o.OrderDate,
                            });

           return orders;
        }
    }
}
