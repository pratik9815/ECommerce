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
        public async Task<ActionResult<List<UserDTO>>> GetUserDetails()
        {
            var users = await _userRepository.GetAdminUsers();
            return Ok(users);
        }
    }
}
