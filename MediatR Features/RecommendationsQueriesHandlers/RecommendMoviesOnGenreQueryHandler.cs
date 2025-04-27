using MediatR;
using MovieAPI.Helper;
using MovieAPI.MediatR_Features.RecommendationQueries;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.RecommendationsQueriesHandlers
{
    public class RecommendMoviesOnGenreQueryHandler(RecommendationRepo _recomRepo,PagenatedMapper _mapper)
        : IRequestHandler<RecommendMoviesOnGenreQuery, RequestResult<PagenatedResponse<MovieResponseDto>>>
    {
        public async Task<RequestResult<PagenatedResponse<MovieResponseDto>>> Handle(
            RecommendMoviesOnGenreQuery request, CancellationToken cancellationToken)
        {
            var repoResult = await _recomRepo.RecommendationsMoviesbasedOnGenreAsync(
                request.PageIndex, request.PageSize, request.UserId);
            
            if (repoResult == null || repoResult.Count==0)
            {
                return RequestResult<PagenatedResponse<MovieResponseDto>>.Failure(404, "No Data Match");
            }
            var data = _mapper.Map<Movie,MovieResponseDto>(repoResult.Data!, request.PageIndex
                , request.PageSize,repoResult.Count);

            return RequestResult<PagenatedResponse<MovieResponseDto>>.Success(data);
        }
    }
}
