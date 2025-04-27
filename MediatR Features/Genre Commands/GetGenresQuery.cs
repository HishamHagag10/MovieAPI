using System.ComponentModel.DataAnnotations;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Genre_Commands
{
    public class GetGenresQuery:IRequest<RequestResult<PagenatedResponse<GenreResponseDto>>>
    {
        public int PageIndex { get; set; } = 1;
        [Range(1,50)]
        public int PageSize { get; set; } = 10;
        
        public string OrderBy { get; set; } = nameof(Actor.Name);
        public string OrderType { get; set; } = Helpers.OrderAscending;
        
    }
}
