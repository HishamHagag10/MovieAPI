using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieAPI.models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Review_Commands
{
    public class UpdateReviewCommand : IRequest<RequestResult<ReviewResponseDto>>
    {
        [BindNever]
        public int Id { get; set; }
        [BindNever]
        public int UserId { get; set; }
        public string? Description { get; set; }
        [Range(0, 10)]
        public double? Rate { get; set; }
    }
}
