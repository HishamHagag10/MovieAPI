namespace MovieAPI.Models.Dtos.Response_Dtos
{
    public class UserMovieResponse
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public DateTime WatchedAt { get; set; }
    }
}
