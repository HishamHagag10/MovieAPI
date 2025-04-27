using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.Dtos
{
    public class MovieActorDto
    {
        [Required]
        public int ActorId { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double Salary { get; set; }
    }
}
