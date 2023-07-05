using DataAccessLayer.Command;
using DataAccessLayer.Common;
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
        public IQueryable<GetOrderCommand> GetProductWithCustomerId(Guid customerId, int page, OrderStatus orderStatus)
        {
            var pageResult = 4f;
            var totalCount = _context.Orders.Where(x => !x.IsDeleted).Count();
            var pageCount = Math.Ceiling(totalCount / pageResult);

            IQueryable<GetOrderCommand> orderDetails = _context.Orders.AsNoTracking()
                                        .Where(x => x.CustomerId == customerId && !x.IsDeleted && x.OrderStatus == orderStatus)
                                        .OrderByDescending(o => o.OrderDate)
                                        .Skip((page - 1) *(int)pageResult).Take((int)pageResult)
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
                    OrderDate = DateTime.UtcNow.AddHours(5).AddMinutes(45),
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
                    var newQuantity = _context.Products.FirstOrDefault(x => x.Id == product.ProductId).Quantity -= product.Quantity;
                    if(newQuantity < 1)
                    {
                        return new ApiResponse
                        {
                            ResponseCode = 500,
                            Message = "Not enough quantity!",
                        };
                    }
                    if (newQuantity == 0)
                    {
                        _context.Products.FirstOrDefault().ProductStatus = ProductStatus.OutOfStock;
                    }
                    else if(newQuantity < 5)
                    {
                        _context.Products.FirstOrDefault().ProductStatus = ProductStatus.LimitedStock;
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
                _context.OrderActivityLogs.Find(orderId).IsDeleted = false;
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

        public async Task<List<GetOrderCommand>> GetOrderWithStatus(OrderStatus orderStatus)
        {
            var orders = await _context.Orders.
                Where(x => !x.IsDeleted &&  x.OrderStatus == orderStatus).
                Select( x => new GetOrderCommand
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    ShippingAddress = x.ShippingAddress,    
                    OrderDate = x.OrderDate,
                    OrdersAddress = x.OrdersAddress,
                    CustomerName = _context.Users.FirstOrDefault(a => a.Id == x.CreatedBy).FullName,
                    OrderEmail = x.OrderEmail,
                    OrderDetails = x.OrderDetails
                                            .Where(od => x.Id == od.OrderId)
                                            .Select(od => new OrderDetails
                                            {
                                                Id = od.Id,
                                                Price = od.Price,
                                                Quantity = od.Quantity,
                                                OrderId = od.OrderId,
                                                Product = _context.Products
                                                                  .FirstOrDefault(p => p.Id == od.ProductId),

                                            }).ToList()
                }).ToListAsync();
            return orders;
        }

        public async Task<int> ChangeOrderStatus(string orderId, OrderStatus orderStatus)
        {
            var order =  _context.Orders.FirstOrDefault(x => x.Id.ToString() == orderId);
            if(order == null)
            {
                return ResponseCodeConstants.NotFound;
            }
            _context.Orders.Find(Guid.Parse(orderId)).OrderStatus = orderStatus;
            _context.OrderActivityLogs.Where(x => x.OrderId.ToString() == orderId).First().OrderStatus = orderStatus;
            await _context.SaveChangesAsync();
            return ResponseCodeConstants.Success;
        }
    }
}
