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
    }
}
