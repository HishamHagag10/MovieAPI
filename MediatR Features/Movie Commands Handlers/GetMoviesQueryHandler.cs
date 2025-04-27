using AutoMapper;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.MediatR_Features.Movie_Commands;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.Movie_Commands_Handlers
{
    public class GetMoviesQueryHandler(MovieRepo _movieRepo, PagenatedMapper _pagenatedMapper) 
        : IRequestHandler<GetMoviesQuery, RequestResult<PagenatedResponse<MovieResponseDto>>>
    {
        public async Task<RequestResult<PagenatedResponse<MovieResponseDto>>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            var repoResponse = await _movieRepo.GetMoviesAsync(
                request.PageIndex,request.PageSize,request.OrderBy,
                request.OrderType,request.ActorId,request.GenreId,request.UserId);

            if (repoResponse == null || repoResponse.Count==0)
            {
                return RequestResult< PagenatedResponse < MovieResponseDto >>
                    .Failure(404,"No Resource Found");
            }
            var data = _pagenatedMapper.Map<Movie, MovieResponseDto>
                (repoResponse.Data!, request.PageIndex, request.PageSize,
                repoResponse.Count);
            return RequestResult<PagenatedResponse<MovieResponseDto>>.Success(data);
        }
    }
}
