using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class OrderActivityLogRepository : IOrderActivityLogRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderActivityLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderActivityLogs(Order order)
        {
            var orderActivityLog = new OrderActivityLog
            {
                Order = order,
                OrderStatus = order.OrderStatus
            };
            _context.OrderActivityLogs.Add(orderActivityLog);
            await _context.SaveChangesAsync();
           
        }
    }
}
