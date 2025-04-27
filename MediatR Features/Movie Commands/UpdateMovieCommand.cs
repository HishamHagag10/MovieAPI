using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Validations;

namespace MovieAPI.MediatR_Features.Movie_Commands
{
    public class UpdateMovieCommand:IRequest<RequestResult<MovieResponseDto>>
    {
        [BindNever]
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Title { get; set; }
        public int? year { get; set; }
        [MaxLength(1000)]
        public string? StoreLine { get; set; }
        [Url]
        public string? Link { get; set; }
        public int? GenreId { get; set; }
        [MaxFileSize(1024 * 1024)]
        [AllowedExtentions(".png,.jpg")]
        public IFormFile? Poster { get; set; }
        //public IEnumerable<MoviesActorsDto>? Actors { get; set; }
    }
}
