using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Genre_Commands
{
    public class GetGenreByIdQuery:IRequest<RequestResult<GenreResponseDto>>
    {
        [Required]
        [FromRoute]
        public int Id { get; set; }
    }
}
