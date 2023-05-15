using DataAccessLayer.Command.ApplicationUser;
using DataAccessLayer.DTO.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<List<UserDTO>> GetAdminUsers();
    }
}
