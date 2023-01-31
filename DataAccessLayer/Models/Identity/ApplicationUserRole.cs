using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.Identity
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        //Relationship mapping for user and role
        public ApplicationUser User { get; set; }
        public ApplicationRole Role { get; set; }
    }
}
