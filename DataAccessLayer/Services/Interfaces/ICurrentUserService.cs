using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services.Interfaces
{
    public interface ICurrentUserService
    {
        public string UserId { get; }
        public string CustomerId { get; }
        public string UserName { get; }
    }
}
