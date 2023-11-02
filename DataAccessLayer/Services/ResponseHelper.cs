using DataAccessLayer.Models;

namespace DataAccessLayer.Services
{
    public class ResponseHelper
    {
        public ApiResponse GetResponse(ResponseFlag flag, string message = null)
        {
            switch (flag)
            {
                case ResponseFlag.Success:
                    return new ApiResponse
                    {
                        ResponseCode = 200,
                        Message = message is null ? "Success" : message.ToString(),
                    };

                case ResponseFlag.Failed:
                    return new ApiResponse
                    {
                        ResponseCode = 400,
                        Message = message is null ? "Failed" : message.ToString()
                    };
                case ResponseFlag.Unauthorized:
                    return new ApiResponse
                    {
                        ResponseCode = 401,
                        Message = message is null ? "Unauthorized Access" : message.ToString()
                    };
            }
            return new ApiResponse
            {
                ResponseCode = 901,
                Message = message is null ? "Something went wrong." : message.ToString(),
            };
        }
    }
}
