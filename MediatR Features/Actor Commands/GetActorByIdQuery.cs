using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Actor_Commands
{
    public class GetActorByIdQuery:IRequest<RequestResult<ActorResponseDto>>
    {
        [Required]
        [FromRoute]
        public int Id { get; set; }
    }
}
