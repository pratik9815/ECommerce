using DataAccessLayer.Common;
using DataAccessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public string UserId
        {
            get
            {
                return _contextAccessor.HttpContext?.User?.FindFirstValue(Constants.Id);
            }
        }
        public string UniqueKey
        {
            get
            {
                return _contextAccessor.HttpContext?.User?.FindFirstValue(Constants.UniqueKey);
            }
        }

        public string UserName
        {
            get
            {
                return _contextAccessor.HttpContext?.User?.FindFirstValue(Constants.UserName);
            }
        }

       

        public string Email
        {
            get
            {
                return _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
            }
        }
        public string Phone
        {
            get
            {
                return _contextAccessor.HttpContext?.User?.FindFirstValue(Constants.PhoneNumber);
            }
        }
        public string Address
        {
            get
            {
                return _contextAccessor.HttpContext?.User?.FindFirstValue(Constants.Address);
            }
        }
        public string UserType
        {
            get
            {
                return _contextAccessor.HttpContext?.User.FindFirstValue(Constants.UserType);
            }
        }
        public string FullName
        {
            get
            {
                return _contextAccessor.HttpContext?.User.FindFirstValue(Constants.FullName);
            }
        }

    }
}
