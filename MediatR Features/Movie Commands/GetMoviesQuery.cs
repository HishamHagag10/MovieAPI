using System.ComponentModel.DataAnnotations;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Movie_Commands
{
    public class GetMoviesQuery:IRequest<RequestResult<PagenatedResponse<MovieResponseDto>>>
    {
        public int PageIndex { get; set; } = 1;
        [Range(1,50)]
        public int PageSize { get; set; } = 10;
        
        public string OrderBy { get; set; } = nameof(Movie.year);
        public string OrderType { get; set; } = Helpers.OrderDescending;
        public int? ActorId { get; set; }
        public int? GenreId { get; set; }
        public int? UserId { get; set; }

    }
}
