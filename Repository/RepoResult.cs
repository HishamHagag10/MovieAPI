
namespace MovieAPI.Repository
{
    public class RepoResult<T>
    {
        private RepoResult(bool isCorrect,ICollection<T> data, int count,string? errorMessage=null)
        {
            Data = data;
            Count = count;
            ErrorMessage = errorMessage??string.Empty;
            IsCorrect = isCorrect;
        }

        public ICollection<T>? Data{ get; set; }
        public string ErrorMessage {  get; set; }
        public int Count { get; set; }
        public bool IsCorrect {  get; set; }
        public static RepoResult<T> ValidationError(string error) => 
            new RepoResult<T>(false,default!,default,error);
        public static RepoResult<T> SuccessValidation() =>
            new RepoResult<T>(true, default!, default, default);

        public static RepoResult<T> Result(ICollection<T> data,int count) =>
            new RepoResult<T>(true, data, count);

    }
}
