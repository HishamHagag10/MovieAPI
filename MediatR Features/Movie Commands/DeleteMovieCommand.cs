using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.models.Dtos.Response_Dtos;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.MediatR_Features.Movie_Commands
{
    public class DeleteMovieCommand : IRequest<RequestResult<MovieResponseDto>>
    {
        [Required]
        [FromRoute]
        public int Id { get; set; }
    }
}
