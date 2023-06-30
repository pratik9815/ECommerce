using DataAccessLayer.Common;
using DataAccessLayer.Common.Order;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECOMMERCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("get-orders")]
        public async Task<ActionResult<List<GetOrderCommand>>> GetAllOrder()
        {
            var order = await _orderRepository.GetAllOrders().ToListAsync();
            return Ok(order);
        }
        [HttpPost("create-orders")]
        public async Task<ActionResult<ApiResponse>> CreateOrders([FromBody] CreateOrderCommand order)
        {
            var response = await _orderRepository.CreateOrder(order);
            if(response.ResponseCode is not 200)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("get-by-id")]
        public async Task<ActionResult<GetOrderCommand>> GetOrderDetails(Guid customerId)
        {
            var orderDetails = await _orderRepository.GetProductWithCustomerId(customerId).ToListAsync();
            return Ok(orderDetails);    
        }
        [HttpPost("remove-order")]
        public async Task<ActionResult> RemoveOrder(Guid orderId)
        {
            var result = await _orderRepository.RemoveOrder(orderId);
            if (result == ResponseCodeConstants.Success)
                return Ok();
            else if(result == ResponseCodeConstants.NotFound)
                return NotFound();
            
            return BadRequest();
           
        }
        [HttpGet("get-order-with-status/{orderStatus}")]
        public async Task<ActionResult<List<GetOrderCommand>>> GetOrderWithOrderStatus([FromRoute]OrderStatus orderStatus)
        {
            var orders = await _orderRepository.GetOrderWithStatus(orderStatus);
            return Ok(orders);  
        }
        [HttpPost("change-order-status/{orderId}")]
        public async Task<ActionResult<ApiResponse>> ChangeOrderStatus(string orderId,[FromBody]OrderStatus orderStatus)
        {
            var result = await _orderRepository.ChangeOrderStatus(orderId,orderStatus);
            if (result == ResponseCodeConstants.Success)
                return Ok();
            else if (result == ResponseCodeConstants.NotFound)
                return NotFound();

            return BadRequest();
        }
    }
}
