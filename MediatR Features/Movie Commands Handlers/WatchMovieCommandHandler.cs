using AutoMapper;
using MediatR;
using MovieAPI.MediatR_Features.Movie_Commands;
using MovieAPI.models;
using MovieAPI.Repository;
using MovieAPI.Repository.UnitOfWork;

namespace MovieAPI.MediatR_Features.Movie_Commands_Handlers
{
    public class WatchMovieCommandHandler(IUnitOfWork _unitOfWork,IMapper _mapper) :
        IRequestHandler<WatchMovieCommand, RequestResult<bool>>
    {
        public async Task<RequestResult<bool>> Handle(WatchMovieCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepo.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return RequestResult<bool>.Failure(404, $"User does not exist");
            }
            var movie =await _unitOfWork.MovieRepo.GetByIdAsync(request.MovieId);
            if (movie == null)
            {
                return RequestResult<bool>.Failure(404,$"No Movie With Id={request.MovieId}");
            }
            var userMovie = _mapper.Map<UserMovies>(request);
            
            await _unitOfWork.UserMovieRepo.AddAsync(userMovie);
            await _unitOfWork.UserMovieRepo.SaveChangesAsync();

            return RequestResult<bool>.Success(true);
        }
    }
}
