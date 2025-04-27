
namespace MovieAPI.Helper
{
    public static class Helpers
    {
        public static string UserRole = "User";
        public static string AdminRole = "Admin";
        public static string OrderAscending = "asc";
        public static string OrderDescending = "desc";

        public static ApiErrorResponse ErrorResponse(int code, string? message=null)
        {
            return new ApiErrorResponse(code,message);
        }
    }
}
