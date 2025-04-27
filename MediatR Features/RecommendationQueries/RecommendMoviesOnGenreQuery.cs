using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieAPI.Helper;
using MovieAPI.models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.RecommendationQueries
{
    public class RecommendMoviesOnGenreQuery : IRequest<RequestResult<PagenatedResponse<MovieResponseDto>>>
    {
        public int PageIndex { get; set; } = 1;
        [Range(1, 50)]
        public int PageSize { get; set; } = 10;
        [BindNever]
        public int UserId { get; set; }

    }
}
