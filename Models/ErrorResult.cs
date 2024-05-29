namespace AcessControlAPI.Models
{
    public class ErrorResult
    {
        public string? Message { get; set; }
        public static ErrorResult ErrorCodeReturner(string? message)
        {
            return new ErrorResult
            {
                Message = message,
            };
        }
    }
}
