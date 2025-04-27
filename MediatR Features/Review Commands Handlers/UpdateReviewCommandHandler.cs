using MediatR;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.models;
using AutoMapper;
using MovieAPI.Repository;
using MovieAPI.MediatR_Features.Review_Commands;

namespace MovieAPI.MediatR_Features.Review_Commands_Handlers
{
    public class UpdateReviewCommandHandler(ReviewRepo _reviewRepo, IMapper _mapper) 
        : IRequestHandler<UpdateReviewCommand, RequestResult<ReviewResponseDto>>
    {
        public async Task<RequestResult<ReviewResponseDto>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review =await _reviewRepo.GetReviewBelongToUser(request.Id,request.UserId);
            if (review == null)
            {
                return RequestResult<ReviewResponseDto>
                    .Failure(404,$"No Review Exist!");
            }

            review = _mapper.Map<UpdateReviewCommand, Review>(request,review);
            
            await _reviewRepo.SaveChangesAsync();

            var data = _mapper.Map<Review, ReviewResponseDto>(review);
            return RequestResult<ReviewResponseDto>.Success(data);
        }
    }
}