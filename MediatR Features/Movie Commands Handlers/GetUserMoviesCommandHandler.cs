using MediatR;
using MovieAPI.Helper;
using MovieAPI.MediatR_Features.Movie_Commands;
using MovieAPI.models;
using MovieAPI.Models.Dtos.Response_Dtos;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.Movie_Commands_Handlers
{
    public class GetUserMoviesCommandHandler (UserMovieRepo _userMovieRepo,PagenatedMapper _mapper):
        IRequestHandler<GetUserMoviesQuery, RequestResult<PagenatedResponse<UserMovieResponse>>>
    {
        public async Task<RequestResult<PagenatedResponse<UserMovieResponse>>> Handle(GetUserMoviesQuery request, CancellationToken cancellationToken)
        {
            var repoResult = await _userMovieRepo.
                GetUserMovies(request.PageIndex, request.PageSize
                ,request.UserId);
            if (repoResult==null || repoResult.Count==0)
            {
                return RequestResult<PagenatedResponse<UserMovieResponse>>.Failure(404, "No Data Match!");
            }
            var data = _mapper.Map<UserMovies,UserMovieResponse>(repoResult.Data!,
                request.PageIndex, request.PageSize, repoResult.Count);

            return RequestResult<PagenatedResponse<UserMovieResponse>>.Success(data);
        }
    }
}
