namespace MovieAPI.models
{
    public class UserMovies
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public DateTime WatchedAt { get; set; }
        public User User { get; set; }
        public Movie Movie { get; set; }
    }
}
