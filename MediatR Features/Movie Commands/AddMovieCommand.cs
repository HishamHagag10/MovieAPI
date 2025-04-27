using System.ComponentModel.DataAnnotations;
using MediatR;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Validations;

namespace MovieAPI.MediatR_Features.Movie_Commands
{
    public class AddMovieCommand:IRequest<RequestResult<MovieResponseDto>>
    {

        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string? StoreLine { get; set; }
        [Required]
        [Url]
        public string? Link { get; set; }
        [Required]
        public int GenreId { get; set; }
        [Required]
        [MaxFileSize(1024 * 1024)]
        [AllowedExtentions(".png,.jpg")]
        public IFormFile? Poster { get; set; }
        
    }
}
