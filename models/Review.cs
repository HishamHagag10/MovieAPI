namespace MovieAPI.models
{
    public class Review
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public double Rate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public User User { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
