using System.ComponentModel.DataAnnotations;
using MediatR;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Award_Commands
{
    public class AddAwardCommand:IRequest<RequestResult<AwardResponseDto>>
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }


    }
}
