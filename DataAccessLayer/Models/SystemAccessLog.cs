using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class SystemAccessLog
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime? LoggedInDateTime { get; set; } 
        public DateTime? LoggedOutDateTime { get; set; }
    }
}
