using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Models.Dtos.Response_Dtos;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.MediatR_Features.Award_Commands
{
    public class DeleteAwardCommand : IRequest<RequestResult<AwardResponseDto>>
    {
        [Required]
        [FromRoute]
        public int Id { get; set; }
    }
}
