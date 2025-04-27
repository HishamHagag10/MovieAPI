namespace MovieAPI.Models.Dtos.Response_Dtos
{
    public class ActorAwardResponseDto
    {
        public int ActorId { get; set; }
        public int AwardId { get; set; }
        public int YearOfHonor { get; set; }
        public string ActorName { get; set; } = string.Empty;
        public string AwardName { get; set; } = string.Empty;

    }
}
