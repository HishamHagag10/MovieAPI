using AutoMapper;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.MediatR_Features.Review_Commands;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Repository;
using MovieAPI.Repository.UnitOfWork;

namespace MovieAPI.MediatR_Features.Review_Commands_Handlers
{
    public class AddReviewCommandHandler(IUnitOfWork _unitOfWork,IMapper _mapper) 
        : IRequestHandler<AddReviewCommand, RequestResult<ReviewResponseDto>>
    {
        public async Task<RequestResult<ReviewResponseDto>> Handle(AddReviewCommand request,
            CancellationToken cancellationToken)
        {
            var movie =await _unitOfWork.MovieRepo
                .GetByIdAsync(request.MovieId);
            if (movie == null)
            {
                return RequestResult<ReviewResponseDto>
                    .Failure(404, $"No Movie with id={request.MovieId}");
            }
            var user = await _unitOfWork.UserRepo.GetByIdAsync(request.MovieId);
            if (user == null)
            {
                return RequestResult<ReviewResponseDto>
                    .Failure(401, $"User does not exist!");
            }
            var review = _mapper.Map<AddReviewCommand,Review>(request);
            await _unitOfWork.ReviewRepo.AddAsync(review);
            movie.SumOfReview += review.Rate;
            ++movie.NoOfReview;
            await _unitOfWork.ReviewRepo.SaveChangesAsync();
            
            var data = _mapper.Map<Review, ReviewResponseDto>(review);
            return RequestResult<ReviewResponseDto>.Success(data);
        }
    }
}
