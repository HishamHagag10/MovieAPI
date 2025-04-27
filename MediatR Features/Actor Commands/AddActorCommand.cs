using System.ComponentModel.DataAnnotations;
using MediatR;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Actor_Commands
{
    public class AddActorCommand:IRequest<RequestResult<ActorResponseDto>>
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public string? Biography { get; set; }
        

    }
}
