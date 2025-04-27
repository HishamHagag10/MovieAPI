using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Award_Commands
{
    public class GetAwardByIdQuery:IRequest<RequestResult<AwardResponseDto>>
    {
        [Required]
        [FromRoute]
        public int Id { get; set; }
    }
}
