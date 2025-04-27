using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.models.Dtos.Response_Dtos;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.MediatR_Features.Actor_Commands
{
    public class DeleteActorCommand : IRequest<RequestResult<ActorResponseDto>>
    {
        [Required]
        [FromRoute]
        public int Id { get; set; }
    }
}
