using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class SystemAccessLogRepository : ISystemAccessLog
    {
        private readonly ApplicationDbContext _context;
       
        public SystemAccessLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetUserLoggedInId(string userId)
        {
            var systemAccess = new SystemAccessLog
            {
                UserId = userId,
                LoggedInDateTime = DateTime.UtcNow.AddHours(5).AddMinutes(45),
                IsLoggedIn = true   
            };
            _context.SystemAccessLogs.Add(systemAccess);
            await _context.SaveChangesAsync();

            return systemAccess.Id.ToString();
        }

        public async Task<bool> CheckIfUserIsLoggedIn( string userId)
        {
            var isLoggedIn = await _context.SystemAccessLogs.AsNoTracking().OrderByDescending(x => x.LoggedInDateTime)
                                           .FirstOrDefaultAsync(x => x.UserId == userId);

            if (isLoggedIn is null) return false;
            return isLoggedIn.IsLoggedIn;
        }

        public async Task SetUserLoggedInToFalse(string systemAccessId, string userId)
        {
            var systemAccessLog = await _context.SystemAccessLogs.AsNoTracking()
                                                  .FirstOrDefaultAsync(x => x.Id == Guid.Parse(systemAccessId) && x.UserId == userId);
            systemAccessLog.LoggedOutDateTime = DateTime.UtcNow.AddHours(5).AddMinutes(45);
            systemAccessLog.IsLoggedIn = false;
            _context.SystemAccessLogs.Update(systemAccessLog);
            await _context.SaveChangesAsync();
        }
    }
}
