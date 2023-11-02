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
        [Authorize]
        public async Task<ActionResult<List<UserDTO>>> GetAdminUser()
        {
            var users = await _userRepository.GetAdminUser();
            return Ok(users);
        }
        [HttpGet("get-customer-user")]
        [Authorize]
        public async Task<ActionResult<List<UserDTO>>> GetCustomerUser()
        {
            var users = await _userRepository.GetCustomer();
            return Ok(users);
        }
        [HttpGet("get-customer-user-by-id/{id}")]
        [Authorize]
        public async Task<ActionResult<List<UserDTO>>> GetCustomerUserById([FromRoute]string id)
        {
            var users = await _userRepository.GetCustomerUsersById(id);
            return Ok(users);
        }

        [HttpGet("get-admin-user-by-id/{id}")]
        [Authorize]
        public async Task<ActionResult<List<UserDTO>>> GetAdminUserById([FromRoute] string id)
        {
            var users = await _userRepository.GetAdminUsersById(id);
            return Ok(users);
        }

        [HttpGet("get-superadmin-user-by-id/{id}")]
        [Authorize]
        public async Task<ActionResult<List<UserDTO>>> GetSuperAdminUserById([FromRoute] string id)
        {
            var users = await _userRepository.GetSuperAdminUsersById(id);
            return Ok(users);
        }
    }
}
