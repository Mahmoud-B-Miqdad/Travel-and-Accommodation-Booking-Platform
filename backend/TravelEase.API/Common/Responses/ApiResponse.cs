namespace TravelEase.API.Common.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string>? Errors { get; set; }

        public ApiResponse() { }

        public ApiResponse(T data, string message = null)
        {
            Success = true;
            Data = data;
            Message = message ?? "Request completed successfully.";
        }

        public ApiResponse(string message)
        {
            Success = false;
            Message = message;
            Data = default;
        }

        public ApiResponse(string message, List<string> errors)
        {
            Success = false;
            Message = message;
            Errors = errors;
            Data = default;
        }

        public static ApiResponse<T> SuccessResponse(T data, string message = null)
        {
            return new ApiResponse<T>(data, message);
        }

        public static ApiResponse<T> FailResponse(string message)
        {
            return new ApiResponse<T>(message);
        }

        public static ApiResponse<T> FailResponse(string message, List<string> errors)
        {
            return new ApiResponse<T>(message, errors);
        }
    }
}