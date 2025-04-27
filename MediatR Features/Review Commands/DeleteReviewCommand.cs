using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieAPI.models.Dtos.Response_Dtos;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.MediatR_Features.Review_Commands
{
    public class DeleteReviewCommand : IRequest<RequestResult<ReviewResponseDto>>
    {
        [Required]
        [FromRoute]
        public int Id { get; set; }
        [BindNever]
        public int UserId { get; set; }
    }
}
