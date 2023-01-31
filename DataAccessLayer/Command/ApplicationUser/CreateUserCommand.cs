using DataAccessLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Command.ApplicationUser
{
    public class CreateUserCommand
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public Gender Gender { get; set; }
        public UserType UserType { get; set; }
        public string Password { get; set; }    
        public string Email { get; set; }   
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
