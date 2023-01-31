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
                return _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }

        public string UserName
        {
            get
            {
                return _contextAccessor.HttpContext?.User?.FindFirstValue(Constants.UserName);
            }
        }

        public string CustomerId
        {
            get
            {
                return _contextAccessor.HttpContext?.User?.FindFirstValue(Constants.CustomerId);
            }
        }
    }
}
