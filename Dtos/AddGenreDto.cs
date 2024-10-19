using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Dtos
{
    public class AddGenreDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
