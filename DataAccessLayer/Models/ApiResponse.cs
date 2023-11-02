namespace DataAccessLayer.Models
{
    public class ApiResponse
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } 
    }

    public static class ResponseCode
    {
        public const int Success = 201;
        public const int Failed = 400;
        public const int Unauthorized = 401;
        public const int Exception = 901;
    }
    public static class ResponseMessage
    {
        public const string Success = "Success";
        public const string Failed = "Failed";
        public const string Unauthorized = "Unauthorized Access";
        public const string Exception = "Something went wrong.";
    }

    public enum ResponseFlag
    {
        Success ,
        Failed, 
        Unauthorized,
        Exception
    }
}
