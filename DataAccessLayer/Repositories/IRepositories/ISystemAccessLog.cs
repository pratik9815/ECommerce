using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface ISystemAccessLog
    {
        Task<string> GetUserLoggedInId(string userId);
        Task<bool> CheckIfUserIsLoggedIn(string userId);
        Task SetUserLoggedInToFalse(string systemAccessId, string userId);
    }
}
