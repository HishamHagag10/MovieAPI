using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Dtos
{
    public class AddAwardDto
    {
        [MaxLength(100)]
        public string Name { get; set; }

    }
}
