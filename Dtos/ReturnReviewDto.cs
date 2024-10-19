using MovieAPI.models;

namespace MovieAPI.Dtos
{
    public class ReturnReviewDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public double Rate { get; set; }
        public string UserName { get; set; }
        public string MovieTitle { get; set; }
    }
}
