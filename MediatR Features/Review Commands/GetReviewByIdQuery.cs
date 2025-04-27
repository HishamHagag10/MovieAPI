using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Review_Commands
{
    public class GetReviewByIdQuery : IRequest<RequestResult<ReviewResponseDto>>
    {
        [Required]
        [FromRoute]
        public int Id { get; set; }
    }
}
