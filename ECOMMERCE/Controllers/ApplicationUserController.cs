﻿using DataAccessLayer.Command.ApplicationUser;
using DataAccessLayer.Common;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Identity;
using DataAccessLayer.Repositories.IRepositories;
using DataAccessLayer.Services;
using DataAccessLayer.Services.Interfaces;
using ECOMMERCE.Common.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        private readonly ISystemAccessLog _systemAccessLog;
        public ApplicationUserController(UserManager<ApplicationUser> userManager,
                                         SignInManager<ApplicationUser> signInManager,
                                         IConfiguration configuration,
                                         ICurrentUserService currentUserService,
                                         ISystemAccessLog systemAccessLog)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _currentUserService = currentUserService;
            _systemAccessLog = systemAccessLog;
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
                Address = command.Address,
                DOB = command.DOB

            };
            var result = await _userManager.CreateAsync(newUser, command.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok(newUser.Id);
        }
        [HttpPost("autheticate-user")]
        public async Task<ActionResult> AuthenticateUser([FromBody] AuthenticateRequest request)
        {
            var identityUser = await _userManager.FindByNameAsync(request.Username);
            if (identityUser == null) return Unauthorized();

            var isUserLoggedIn = await _systemAccessLog.CheckIfUserIsLoggedIn( identityUser.Id);

            if (isUserLoggedIn)
                return Unauthorized();


            var result = await _signInManager.CheckPasswordSignInAsync(identityUser, request.Password, false);
            if (!result.Succeeded) return Unauthorized();

            var tokenDetails = GetTokenDetail(identityUser, await _systemAccessLog.GetUserLoggedInId(identityUser.Id));

            var token = TokenGenerator.GenerateToken(tokenDetails, _configuration);

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
            if (!result.Succeeded) return BadRequest(result.Errors);
            return Ok();
        }

        [HttpPost("change-password")]
        public async Task<ApiResponse> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = _currentUserService?.UserId;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return new ApiResponse
                {
                    ResponseCode = 400,
                    Message = "BadRequest"
                };

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

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


        [HttpPost("logout")]
        public async Task<ActionResult> LogOut()
        {
            var userId = _currentUserService.UserId;
            var systemAccessLogId = _currentUserService.UniqueKey;
            if (userId is null || systemAccessLogId is null)
                return BadRequest();
            await _systemAccessLog.SetUserLoggedInToFalse(systemAccessLogId, userId);
            return Ok();
        }
        private TokenGenerateDetail GetTokenDetail(ApplicationUser user, string uniqueKey)
        {
            return new TokenGenerateDetail
            {
                UserId = user.Id,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Address = user.Address,
                FullName = user.FullName,
                UniqueKey = uniqueKey,
                UserType = user.UserType
            };
        }


    }

    public class TokenGenerateDetail
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public UserType UserType { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public string UniqueKey { get; set; }


    }
}
