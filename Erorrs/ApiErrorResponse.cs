namespace MovieAPI.Erorrs
{
    public class ApiErrorResponse
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public ApiErrorResponse(int code,string? message=null) 
        {
            ErrorCode = code;
            ErrorMessage = message ?? DefaultMessage(code);
        }
        string DefaultMessage(int code)
        {
            return code switch
            {
                400 => "Bad Request.",
                401 => "You are not Authorized.",
                403 => "Forbidden to do that!",
                404 => "Data Not Found.",
                500 => "Server Error! Try Again later.",
                _ => "Unexpecting Error."
            };
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
