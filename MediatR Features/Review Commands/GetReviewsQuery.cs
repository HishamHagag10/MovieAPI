using System.ComponentModel.DataAnnotations;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Review_Commands
{
    public class GetReviewsQuery : IRequest<RequestResult<PagenatedResponse<ReviewResponseDto>>>
    {
        public int PageIndex { get; set; } = 1;
        [Range(1,50)]
        public int PageSize { get; set; } = 10;
        
        public string OrderBy { get; set; } = nameof(Review.UpdatedAt);
        public string OrderType { get; set; } = Helpers.OrderDescending;
        public int? MovieId { get; set; }
        public int? UserId {  get; set; }


    }
}
