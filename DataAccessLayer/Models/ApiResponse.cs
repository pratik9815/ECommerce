using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ApiResponse
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } 
    }
}
