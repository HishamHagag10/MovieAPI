using MediatR;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.models;
using AutoMapper;
using MovieAPI.Repository;
using MovieAPI.MediatR_Features.Movie_Commands;

namespace MovieAPI.MediatR_Features.Movie_Commands_Handlers
{
    public class UpdateMovieCommandHandler(MovieRepo _movieRepo, IMapper _mapper) 
        : IRequestHandler<UpdateMovieCommand, RequestResult<MovieResponseDto>>
    {
        public async Task<RequestResult<MovieResponseDto>> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            var movie =await _movieRepo.GetByIdAsync(request.Id);
            
            if (movie == null)
            {
                return RequestResult<MovieResponseDto>.Failure(404,"No Movie with this Id");
            }
            
            movie = _mapper.Map<UpdateMovieCommand, Movie>(request,movie);
            
            var validation = await _movieRepo.ValidateMovieAsync(movie);
            if (!validation.IsCorrect)
            {
                return RequestResult<MovieResponseDto>
                    .Failure(400, "Duplicate Data Ensure that link and title of the movie are unique");
            }

            if (request.Poster != null)
            {
                using var dataStream = new MemoryStream();
                await request.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }

            await _movieRepo.SaveChangesAsync();
            
            var data = _mapper.Map<Movie, MovieResponseDto>(movie);
            return RequestResult<MovieResponseDto>.Success(data);
        }
    }
}