using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieAPI.models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Actor_Commands
{
    public class UpdateActorCommand:IRequest<RequestResult<ActorResponseDto>>
    {
        [BindNever]
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string? Name { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Biography { get; set; }
    }
}
