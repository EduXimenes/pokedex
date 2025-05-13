namespace Pokedex.Domain.Dto
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public static ApiResponse<T> Ok(T data, string message = "Success")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Errors = new List<string>()
            };
        }

        public static ApiResponse<T> Error(List<string> errors, string message = "Error")
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Errors = errors
            };
        }
    }
} 