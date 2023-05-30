using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECOMMERCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailsRepository _orderDetailsRepository;

        public OrderDetailsController(IOrderDetailsRepository orderDetailsRepository)
        {
            _orderDetailsRepository = orderDetailsRepository;
        }

        [HttpGet("get-order-details")]
        public async Task<ActionResult<OrderDetails>> GetOrderDetails(Guid customerId)
        {
            var orderDetails = await _orderDetailsRepository.GetOrderDetails(customerId).ToListAsync();
            return Ok(orderDetails);
        }
    }
}
