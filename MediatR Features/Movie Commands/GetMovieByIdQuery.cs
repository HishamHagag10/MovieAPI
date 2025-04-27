using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Movie_Commands
{
    public class GetMovieByIdQuery: IRequest<RequestResult<MovieResponseDto>>
    {
        [Required]
        [FromRoute]
        public int Id { get; set; }
    }
}
