using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Genre_Commands
{
    public class UpdateGenreCommand:IRequest<RequestResult<GenreResponseDto>>
    {
        [BindNever]
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string? Name { get; set; }
    }
}
