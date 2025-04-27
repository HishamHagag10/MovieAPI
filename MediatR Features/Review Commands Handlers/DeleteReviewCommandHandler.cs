using AutoMapper;
using MediatR;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.models;
using MovieAPI.Repository;
using MovieAPI.MediatR_Features.Review_Commands;

namespace MovieAPI.MediatR_Features.Review_Commands_Handlers
{
    public class DeleteReviewCommandHandler(ReviewRepo _reviewRepo, IMapper _mapper) 
        : IRequestHandler<DeleteReviewCommand, RequestResult<ReviewResponseDto>>
    {
        public async Task<RequestResult<ReviewResponseDto>> Handle(DeleteReviewCommand request,
            CancellationToken cancellationToken)
        {
            var review = await _reviewRepo.GetReviewBelongToUser(request.Id, request.UserId);
            if (review == null)
            {
                return RequestResult<ReviewResponseDto>
                    .Failure(404,$"No Review Exist!");
            }
            _reviewRepo.Delete(review);
            await _reviewRepo.SaveChangesAsync();
            var data= _mapper.Map<Review, ReviewResponseDto>(review);
            return RequestResult<ReviewResponseDto>.Success(data);
        }
    }
}
