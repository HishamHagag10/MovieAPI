using MovieAPI.models;

namespace MovieAPI.Dtos
{
    public class AddReviewDto
    {
        public string? Description { get; set; }
        public double Rate { get; set; }
        public int MovieId { get; set; }
    }
}
