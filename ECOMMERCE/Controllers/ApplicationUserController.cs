using DataAccessLayer.Command.ApplicationUser;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models.Identity;
using ECOMMERCE.Common.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECOMMERCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        public ApplicationUserController(UserManager<ApplicationUser> userManager,
                                         SignInManager<ApplicationUser> signInManager,
                                         IConfiguration configuration)
        {
            _userManager= userManager;
            _signInManager= signInManager;
            _configuration = configuration;
        }
        [HttpPost("create-user")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var newUser = new ApplicationUser 
            { 
                UserName = command.UserName,
                FullName = command.FullName,
                Gender= command.Gender,
                Email = command.Email,
                UserType = command.UserType,
                PhoneNumber = command.PhoneNumber, 
                Address = command.Address ,
            };
            var result = await _userManager.CreateAsync(newUser, command.Password);
            if(!result.Succeeded)
                     return BadRequest(result.Errors);
            return Ok();
        }
        [HttpPost("autheticate-user")]
        public async Task<ActionResult> AuthenticateUser([FromBody] AuthenticateRequest request)
        {
            var identityUser = await _userManager.FindByNameAsync(request.Username);
            if (identityUser == null)  return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(identityUser, request.Password, false);
            if (!result.Succeeded) return Unauthorized();

            var token = TokenGenerator.GenerateToken(identityUser, _configuration);

            return Ok(new
            {
                token = token,
            });
        }
    }
}
