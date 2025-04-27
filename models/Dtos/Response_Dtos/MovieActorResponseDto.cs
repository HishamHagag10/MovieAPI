using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.Dtos.Response_Dtos
{
    public class MovieActorResponseDto
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public double Salary { get; set; }
    }
}
