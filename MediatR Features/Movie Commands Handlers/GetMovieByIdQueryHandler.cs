using AutoMapper;
using MediatR;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.MediatR_Features.Movie_Commands;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.Movie_Commands_Handlers
{
    public class GetMovieByIdQueryHandler(MovieRepo _movieRepo,IMapper _mapper) 
        : IRequestHandler<GetMovieByIdQuery, RequestResult<MovieResponseDto>>
    {
        public async Task<RequestResult<MovieResponseDto>> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await _movieRepo.GetByIdAsync(request.Id);
            if (movie == null)
            {
                return RequestResult<MovieResponseDto>.Failure(404,"No Movie with this Id");
            }
            var data = _mapper.Map<Movie, MovieResponseDto>(movie);
            return RequestResult<MovieResponseDto>.Success(data);
        }
    }
}
