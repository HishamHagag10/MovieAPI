using MovieAPI.Helper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MovieAPI.MediatR_Features
{
    public class RequestResult<T>
    {
        public int StatusCode { get; }
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }
        public T Data { get; set; }
        private RequestResult(bool isSuccess,T data, int? statusCode=null ,string? errorMessage=null)
        {
            StatusCode = statusCode ?? 200;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Data = data;
        }
        public static RequestResult<T> Success(T data)=>new RequestResult<T>(true,data);
        public static RequestResult<T> Failure(int statusCode, string errorMessage) 
            => new RequestResult<T>(false, default!, statusCode,errorMessage);

    }

}
