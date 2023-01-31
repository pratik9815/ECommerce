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
    public class ApplicationRole : IdentityRole , IAuditableEntity
    {
        public RoleType RoleType { get; set; }
        //Relationship  both the applicatonuser and applicationuserRole
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        //
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
