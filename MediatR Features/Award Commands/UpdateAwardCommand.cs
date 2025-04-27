using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieAPI.Models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Award_Commands
{
    public class UpdateAwardCommand:IRequest<RequestResult<AwardResponseDto>>
    {
        [BindNever]        
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string? Name { get; set; }
    }
}
