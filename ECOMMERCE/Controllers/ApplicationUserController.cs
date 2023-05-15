using DataAccessLayer.Command.ApplicationUser;
using DataAccessLayer.Common;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Identity;
using DataAccessLayer.Services;
using DataAccessLayer.Services.Interfaces;
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
        private readonly ICurrentUserService _currentUserService;
        public ApplicationUserController(UserManager<ApplicationUser> userManager,
                                         SignInManager<ApplicationUser> signInManager,
                                         IConfiguration configuration,
                                         ICurrentUserService currentUserService)
        {
            _userManager= userManager;
            _signInManager= signInManager;
            _configuration = configuration;
            _currentUserService= currentUserService;
        }
        [HttpPost("create-user")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var newUser = new ApplicationUser 
            { 
                UserName = command.UserName,
                FullName = command.FullName,
                Gender = command.Gender,
                Email = command.Email,
                UserType = command.UserType,
                PhoneNumber = command.PhoneNumber, 
                Address = command.Address ,
                DOB = command.DOB
                
            };
            var result = await _userManager.CreateAsync(newUser, command.Password);
            if(!result.Succeeded)
                     return BadRequest(result.Errors);
            return Ok(newUser.Id);
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
                userID = identityUser.Id
            });
        }


       

        [HttpPut("update-user")]
        public async Task<ActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            var userType = _currentUserService?.UserType;
            var user = await _userManager.FindByIdAsync(command.Id);

            user.PhoneNumber = command.PhoneNumber;
            user.Address = command.Address;
            user.UserName = command.UserName;
            user.FullName = command.FullName;
            user.Email = command.Email;
            user.DOB = command.DOB;
            user.Gender = command.Gender;

            IdentityResult result = await _userManager.UpdateAsync(user); 
            if(!result.Succeeded) return BadRequest(result.Errors);    
            return Ok();
        }

        [HttpPost("change-password")]
        public async Task<ApiResponse> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = _currentUserService?.UserId;
            var user = await  _userManager.FindByIdAsync(userId);

            if (user == null) 
                return new ApiResponse
                    {
                        ResponseCode = 400,
                        Message = "BadRequest"
                    };
        

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword,request.NewPassword);

            if (!result.Succeeded)
                return new ApiResponse
                    {
                        ResponseCode = 401,
                        Message = "Old password is wrong."
                    };


            return new ApiResponse
            {
                ResponseCode = 200,
                Message = "Password changed successfully."
            };
        }
    }
}
