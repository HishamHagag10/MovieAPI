using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieAPI.models;
 
using MovieAPI.models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Review_Commands
{
    public class AddReviewCommand :
        IRequest<RequestResult<ReviewResponseDto>>
    {
        public string? Description { get; set; }
        [Required]
        [Range(0, 10)]
        public double Rate { get; set; }
        [Required]
        public int MovieId { get; set; }
        [BindNever]
        public int UserId { get; set; }


    }
}
