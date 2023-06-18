using DataAccessLayer.Command;
using DataAccessLayer.Common.Order;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using DataAccessLayer.Query.Product;
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
        private readonly IOrderActivityLogRepository _orderActivityLogRepository;

        public OrderRepository(ApplicationDbContext context, ICurrentUserService currentUserService,IOrderActivityLogRepository orderActivityLogRepository)
        {
            _context = context;
            _currentUserService = currentUserService;
            _orderActivityLogRepository = orderActivityLogRepository;
        }

        public IQueryable<GetOrderCommand> GetAllOrders()
        {
            var orders = _context.Orders.
                            OrderByDescending(o => o.OrderDate).   
                            AsNoTracking().
                            Where(o => o.IsDeleted == false).
                            Select(o => new GetOrderCommand
                            {
                                Id = o.Id,
                                OrdersAddress = o.OrdersAddress,
                                ShippingAddress = o.ShippingAddress,
                                Amount = o.Amount,
                                OrderEmail = o.OrderEmail,
                                OrderDate = o.OrderDate,
                                CustomerId = o.CustomerId.ToString(),
                                CreatedBy = _context.Users.FirstOrDefault(a => a.Id == o.CreatedBy).FullName,                           
                                OrderStatus = o.OrderStatus,
                                OrderDetails = o.OrderDetails
                                            .Where(od => o.Id == od.OrderId)
                                            .Select(od => new OrderDetails
                                            {
                                                Id = od.Id,
                                                Price = od.Price,
                                                Quantity = od.Quantity,
                                                OrderId = od.OrderId,
                                                Product = _context.Products
                                                                  .FirstOrDefault(p => p.Id == od.ProductId),
                                                
                                            }).ToList()
                                            });

            return orders;
        }
        //Get orders by customer id
        public IQueryable<GetOrderCommand> GetProductWithCustomerId(Guid customerId)
        {

            var orderDetails = _context.Orders.AsNoTracking()
                                        .Where(x => x.CustomerId == customerId && x.IsDeleted == false)
                                        .Select(x => new GetOrderCommand
                                        {
                                            Id = x.Id,
                                            OrdersAddress = x.OrdersAddress,
                                            ShippingAddress = x.ShippingAddress,
                                            Amount = x.Amount,
                                            OrderEmail = x.OrderEmail,      
                                            OrderDate = x.OrderDate,
                                            OrderStatus = x.OrderStatus,
                                            OrderDetails = x.OrderDetails
                                            .Where(od => x.Id == od.OrderId)
                                            .Select(od => new OrderDetails
                                            {
                                                Id=od.Id,
                                                Price = od.Price,
                                                Quantity = od.Quantity,
                                                OrderId = od.OrderId,
                                                Product = _context.Products
                                                                  .FirstOrDefault(p => p.Id == od.ProductId),
                                                //ProductId = od.ProductId
                                            }).ToList(), 
                                        });
            return orderDetails;
        }
        public async Task<ApiResponse> CreateOrder(CreateOrderCommand order)
        {
            try
            {
                var customerId = _currentUserService.CustomerId;
                
                var newOrder = new Order
                {
                    Amount = order.Amount,
                    OrderDate = DateTime.UtcNow,
                    OrderEmail = order.OrderEmail,
                    OrdersAddress = order.OrdersAddress,
                    ShippingAddress = order.ShippingAddress,
                    PhoneNumber = order.PhoneNumber,
                    CustomerId = Guid.Parse(customerId),
                    CreatedBy = _currentUserService.FullName,
                    CreatedDate = DateTime.UtcNow.AddHours(5).AddMinutes(45),
                    OrderStatus  = Common.OrderStatus.Pending,
                };
                foreach(var product in order.Product)
                {
                    var newQuanitity = _context.Products.FirstOrDefault(x => x.Id == product.ProductId).Quantity -= product.Quantity;
                    if(newQuanitity < 1)
                    {
                        return new ApiResponse
                        {
                            ResponseCode = 500,
                            Message = "Not enough quantity!",
                        };
                    }
                    newOrder.AddOrder(newOrder,product);
                }
                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();
                await _orderActivityLogRepository.AddOrderActivityLogs(newOrder);

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

        public async Task<int> RemoveOrder(Guid orderId)
        {
            
            try
            {
                var delOrder = await _context.Orders.FindAsync(orderId);
                if (delOrder == null)
                {
                    return ResponseCodeConstants.NotFound;
                }

                var product = await _context.Orders
                                                        .Where(x => x.Id == orderId)
                                                        .Select(x => new DeleteOrderCommand
                                                        {
                                                            quantity = x.OrderDetails.Select(x => x.Quantity).FirstOrDefault(),
                                                            ProductId = x.OrderDetails.Select(x => x.ProductId).FirstOrDefault(),
                                                        }).ToListAsync();

                foreach (var delCommand in product)
                {
                     _context.Products
                    .FirstOrDefault(x => x.Id == delCommand.ProductId)
                    .Quantity += delCommand.quantity;
                    
                    await _context.SaveChangesAsync();
                }
                _context.OrderDetails.Where(x => x.OrderId == orderId).FirstOrDefault().IsDeleted = false;
                delOrder.IsDeleted = true;
                delOrder.DeletedBy = _currentUserService?.CustomerId;
                delOrder.DeletedDate = DateTime.UtcNow.AddHours(5).AddMinutes(45);
                await _context.SaveChangesAsync();
                return ResponseCodeConstants.Success;
            }
            catch
            {
                return ResponseCodeConstants.Exception;
            }
        }
    }
}
