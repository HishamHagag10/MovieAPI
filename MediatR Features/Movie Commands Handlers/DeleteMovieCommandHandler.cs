using AutoMapper;
using MediatR;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.models;
using MovieAPI.Repository;
using MovieAPI.MediatR_Features.Movie_Commands;

namespace MovieAPI.MediatR_Features.Movie_Commands_Handlers
{
    public class DeleteMovieCommandHandler(MovieRepo _movieRepo, IMapper _mapper) 
        : IRequestHandler<DeleteMovieCommand, RequestResult<MovieResponseDto>>
    {
        public async Task<RequestResult<MovieResponseDto>> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = await _movieRepo.GetByIdAsync(request.Id);
            if (movie == null)
            {
                return RequestResult<MovieResponseDto>.Failure(404,"No Movie with this Id");
            }
            _movieRepo.Delete(movie);
            await _movieRepo.SaveChangesAsync();
            var data = _mapper.Map<Movie, MovieResponseDto>(movie);
            return RequestResult<MovieResponseDto>.Success(data); 
        }
    }
}
