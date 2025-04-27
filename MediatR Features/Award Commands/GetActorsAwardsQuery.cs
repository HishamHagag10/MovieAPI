using MediatR;
using MovieAPI.Helper;
using MovieAPI.models;
using MovieAPI.Models.Dtos.Response_Dtos;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.MediatR_Features.Award_Commands
{
    public class GetActorsAwardsQuery : IRequest<RequestResult<PagenatedResponse<ActorAwardResponseDto>>>
    {
        public int PageIndex { get; set; } = 1;
        [Range(1, 50)]
        public int PageSize { get; set; } = 10;
        public string OrderType { get; set; } = Helpers.OrderDescending;
        public int? ActorId { get; set; }
        public int? YearOfHonor { get; set; }
        public int? AwardId { get; set; }
    }
}
