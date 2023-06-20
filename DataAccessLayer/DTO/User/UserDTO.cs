using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.User
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string UserType { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string Image { get; set; }
        public string CreatedBy { get; set; }   
        public string CustomerId { get; set; }

       
    }
}
