using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieAPI.models;
using MovieAPI.Models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Award_Commands
{
    public class AddAwardToActorCommand:IRequest<RequestResult<ActorAwardResponseDto>>
    {
        [BindNever]
        public int ActorId { get; set; }
        [BindNever]
        public int AwardId { get; set; }
        [Required]
        public int YearOfHonor { get; set; }
        
    }
}
