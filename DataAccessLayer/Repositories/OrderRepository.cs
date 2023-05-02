using DataAccessLayer.Common.Order;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using DataAccessLayer.Services.Interfaces;
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
        private readonly ICurrentUserService _currentUserService;

        public OrderRepository(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public IQueryable<GetOrderCommand> GetAllOrders()
        {
            var orders = _context.Orders.
                            OrderByDescending(o => o.OrderDate).
                            Where(o => o.IsDeleted == false).
                            Select(o => new GetOrderCommand
                            {
                                Id = o.Id,
                                OrdersAddress = o.OrdersAddress,
                                ShippingAddress = o.ShippingAddress,
                                Amount = o.Amount,
                                OrderEmail = o.OrderEmail,
                                OrderDate = o.OrderDate,
                                CustomerId = _currentUserService.UserId,

                                CreatedBy = _context.Users.FirstOrDefault(a => a.Id == o.CreatedBy).FullName,
                            });

            return orders;
        }

        public async Task<ApiResponse> CreateOrder(CreateOrderCommand order)
        {
            try
            {
                var newOrder = new Order
                {
                    Amount = order.Amount,
                    OrderDate = DateTime.UtcNow,
                    OrderEmail = _currentUserService.Email,
                    OrdersAddress = _currentUserService.Address,
                    ShippingAddress = order.ShippingAddress,
                    PhoneNumber = _currentUserService.Phone,
                };

                newOrder.AddOrder(newOrder, order.ProductId);
                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();
                return new ApiResponse
                {
                    ResponseCode = 200,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                var errors = new List<string>();
                errors.Add(ex.Message);
                return new ApiResponse
                {
                    ResponseCode = 500,
                    Message = "Failed",
                    Errors = errors
                };
            }
        }
    }
}
