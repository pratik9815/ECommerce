using DataAccessLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Command.ApplicationUser
{
    public class UpdateUserCommand
    {
        public string Id { get; set; }        
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public Gender Gender { get; set; }  
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }

    }
}
