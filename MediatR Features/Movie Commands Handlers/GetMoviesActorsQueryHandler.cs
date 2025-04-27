using AutoMapper;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.MediatR_Features.Movie_Commands;
using MovieAPI.models;
using MovieAPI.Models.Dtos.Response_Dtos;
using MovieAPI.Repository.UnitOfWork;

namespace MovieAPI.MediatR_Features.Movie_Commands_Handlers
{
    public class GetMoviesActorsQueryHandler (IUnitOfWork _unitOfWork,PagenatedMapper _mapper) :
        IRequestHandler<GetMoviesActorsQuery, RequestResult<PagenatedResponse<MovieActorResponseDto>>>
    {
        async Task<RequestResult<PagenatedResponse<MovieActorResponseDto>>> IRequestHandler<GetMoviesActorsQuery, RequestResult<PagenatedResponse<MovieActorResponseDto>>>.Handle(GetMoviesActorsQuery request, CancellationToken cancellationToken)
        {
            var repoResponse = await _unitOfWork.MovieActorsRepo
                .GetActorsMoviesAsync(request.PageIndex,request.PageSize,request.ActorId,request.MovieId);
            
            if (repoResponse == null || repoResponse.Count == 0)
                return RequestResult<PagenatedResponse<MovieActorResponseDto>>
                    .Failure(404,"No Data Matches");

            var data = _mapper.Map<MoviesActors, MovieActorResponseDto>
                (repoResponse.Data!,request.PageIndex,request.PageSize,repoResponse.Count);

            return RequestResult<PagenatedResponse<MovieActorResponseDto>>.Success(data);
        }
    }
}
