using DataAccessLayer.Common;
using DataAccessLayer.Models.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.Identity
{
    public class ApplicationUser : IdentityUser, IAuditableEntity 
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public DateTime DOB { get; set; }
        public UserType UserType { get; set; }
        public Guid? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<ApplicationRole> Roles { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }


    }
}
