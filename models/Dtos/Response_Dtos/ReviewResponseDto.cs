namespace MovieAPI.models.Dtos.Response_Dtos
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public double Rate { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
