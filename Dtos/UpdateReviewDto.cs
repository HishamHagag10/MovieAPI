using MovieAPI.models;

namespace MovieAPI.Dtos
{
    public class UpdateReviewDto
    {
        public string? Description { get; set; }
        public double? Rate { get; set; }
    }
}
