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
    public class AddMovieCommandHandler(MovieRepo _movieRepo,GenreRepo _genreRepo,IMapper _mapper) 
        : IRequestHandler<AddMovieCommand, RequestResult<MovieResponseDto>>
    {
        public async Task<RequestResult<MovieResponseDto>> Handle(AddMovieCommand request, 
            CancellationToken cancellationToken)
        {
            var genre = await _genreRepo.GetByIdAsync(request.GenreId);
            if (genre == null)
            {
                return RequestResult<MovieResponseDto>
                    .Failure(400, "No Genre With this Id");
            }
            var movie = _mapper.Map<AddMovieCommand,Movie>(request);

            var validation = await _movieRepo.ValidateMovieAsync(movie);
            if (!validation.IsCorrect)
            {
                return RequestResult<MovieResponseDto>
                    .Failure(400, validation.ErrorMessage!);
            }

            using var dataStream = new MemoryStream();
            await request.Poster.CopyToAsync(dataStream);
            movie.Poster = dataStream.ToArray();
            
            movie=await _movieRepo.AddAsync(movie);
            await _movieRepo.SaveChangesAsync();

            movie.Genre = genre;

            var data = _mapper.Map<Movie, MovieResponseDto>(movie);

            return RequestResult<MovieResponseDto>.Success(data);
        }
    }
}
