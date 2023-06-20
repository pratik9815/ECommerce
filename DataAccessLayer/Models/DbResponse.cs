using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public static class ResponseCodeConstants
    {
        public const int Success = 0 ;
        public const int Failed = 1 ;  
        public const int Exception = 2 ;
        public const int NotFound = 3;
        public const int Unauthorized = 4;
    }

    public static class ResponseMessageConstants
    {
        public const string Success = "Success" ;   
        public const string Failed = "Failed";   
        public const string Exception = "Something went wrong" ;   
        public const string NotFound = "Not found" ;   
        public const string Unauthorized = "Unauthorized";   
    }
}
