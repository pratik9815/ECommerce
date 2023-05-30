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
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailsRepository(ApplicationDbContext context)
        {
            _context = context; 
        }

        public  IQueryable<OrderDetails> GetOrderDetails(Guid customerId)
        {
            var orderDetails = _context.OrderDetails.AsNoTracking()
                                                      .Where(x => x.Order.CustomerId == customerId)
                                                      .Select(od => new OrderDetails
                                                      {
                                                          Id = od.Id,   
                                                          Price = od.Price,
                                                          Quantity = od.Quantity,
                                                          ProductId = od.ProductId,
                                                          Product = od.Product,
                                                          OrderId = od.OrderId,
                                                          Order = od.Order
                                                      }); 
            return orderDetails;
        }
        
    }
}
