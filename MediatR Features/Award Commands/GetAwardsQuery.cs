using System.ComponentModel.DataAnnotations;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.models;
using MovieAPI.Models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Award_Commands
{
    public class GetAwardsQuery:IRequest<RequestResult<PagenatedResponse<AwardResponseDto>>>
    {
        public int PageIndex { get; set; } = 1;
        [Range(1,50)]
        public int PageSize { get; set; } = 10;
        public string OrderType { get; set; } = Helpers.OrderAscending;
    }
}
