using DataAccessLayer.DTO.User;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECOMMERCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("get-user")]
        [Authorize]
        public async Task<ActionResult<List<UserDTO>>> GetAllAdminUsers()
        {
            var users = await _userRepository.GetAllAdminUsers();
            return Ok(users);
        }
        [HttpGet("get-superadmin-user")]
        [Authorize]
        public async Task<ActionResult<List<UserDTO>>> GetSuperAdminUserDetails()
        {
            var users = await _userRepository.GetSuperAdminUsers();
            return Ok(users);
        }
        [HttpGet("get-admin-user")]
        public async Task<ActionResult<List<UserDTO>>> GetAdminUser()
        {
            var users = await _userRepository.GetAdminUser();
            return Ok(users);
        }
        [HttpGet("get-customer-user")]
        public async Task<ActionResult<List<UserDTO>>> GetCustomerUser()
        {
            var users = await _userRepository.GetCustomer();
            return Ok(users);
        }
    }
}
