using System.ComponentModel.DataAnnotations;
using MediatR;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Genre_Commands
{
    public class AddGenreCommand:IRequest<RequestResult<GenreResponseDto>>
    {
        [Required]
        public string? Name { get; set; }

    }
}
