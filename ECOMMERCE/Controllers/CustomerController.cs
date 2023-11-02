using DataAccessLayer.Common.Customer;
using DataAccessLayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace ECOMMERCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<GetCustomerCommand>> GetCustomerById([FromRoute]string id)
        {
            var customer = await _customerRepository.GetCustomerById(Guid.Parse(id));
            return Ok(customer);
        }
    }
}
