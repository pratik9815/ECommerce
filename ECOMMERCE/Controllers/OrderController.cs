using DataAccessLayer.Common.Order;
using DataAccessLayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECOMMERCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

    }
}
