using AutoMapper;
using MediatR;
using MovieAPI.MediatR_Features.Movie_Commands;
using MovieAPI.models;
using MovieAPI.Models.Dtos;
using MovieAPI.Models.Dtos.Response_Dtos;
using MovieAPI.Repository.UnitOfWork;

namespace MovieAPI.MediatR_Features.Movie_Commands_Handlers
{
    public class AddActorsToMovieCommandHandler (IUnitOfWork _unitOfWork,IMapper _mapper): 
        IRequestHandler<AddActorsToMovieCommand,
            RequestResult<bool>>
    {
        public async Task<RequestResult<bool>> Handle(AddActorsToMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = await _unitOfWork.MovieRepo.GetByIdAsync(request.MovieId);
            if (movie == null)
                return RequestResult<bool>
                    .Failure(404, "NO Movie");
            
            var validate = await _unitOfWork.ActorRepo.CheckActorsIdsExist(request.Actors.Select(a=>a.ActorId));
            if (!validate.IsCorrect)
                return RequestResult<bool>
                    .Failure(400, validate.ErrorMessage);

            var movieActors = request.ToModel(); //_mapper.Map<IEnumerable<MoviesActors>>(request);
            
            await _unitOfWork.MovieActorsRepo.AddRangeAsync(movieActors);
            await _unitOfWork.MovieActorsRepo.SaveChangesAsync();

            //_mapper.Map<MovieActorsResponseDto>(movieActors);

            return RequestResult<bool>.Success(true);
        }
    }
}
