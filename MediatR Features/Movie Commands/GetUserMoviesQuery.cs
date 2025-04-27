using System.ComponentModel.DataAnnotations;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.models;
using MovieAPI.Models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Movie_Commands
{
    public class GetUserMoviesQuery:IRequest<RequestResult
        <PagenatedResponse<UserMovieResponse>>>
    {
        public int PageIndex { get; set; } = 1;
        [Range(1, 50)]
        public int PageSize { get; set; } = 10;
        public int UserId { get; set; }
    }
}
