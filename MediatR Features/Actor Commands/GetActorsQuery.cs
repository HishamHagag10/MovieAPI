using System.ComponentModel.DataAnnotations;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Actor_Commands
{
    public class GetActorsQuery:IRequest<RequestResult<PagenatedResponse<ActorResponseDto>>>
    {
        public int PageIndex { get; set; } = 1;
        [Range(1,50)]
        public int PageSize { get; set; } = 10;
        
        public string OrderBy { get; set; } = nameof(Actor.Name);
        public string OrderType { get; set; } = Helpers.OrderAscending;
        public int? MovieId { get; set; }
        public int? AwardId {  get; set; }
    }
}
