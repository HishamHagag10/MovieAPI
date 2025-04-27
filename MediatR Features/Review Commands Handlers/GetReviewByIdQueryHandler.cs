using AutoMapper;
using MediatR;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.MediatR_Features.Review_Commands;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.Review_Commands_Handlers
{
    public class GetReviewByIdQueryHandler(ReviewRepo _reviewRepo,IMapper _mapper) 
        : IRequestHandler<GetReviewByIdQuery, RequestResult<ReviewResponseDto>>
    {
        public async Task<RequestResult<ReviewResponseDto>> Handle(GetReviewByIdQuery request,
            CancellationToken cancellationToken)
        {
            var review = await _reviewRepo.GetByIdAsync(request.Id);
            if (review == null)
            {
                return RequestResult<ReviewResponseDto>
                    .Failure(404,$"No Review with Id={request.Id}");
            }
            var data= _mapper.Map<Review, ReviewResponseDto>(review);
            return RequestResult<ReviewResponseDto>.Success(data);
        }
    }
}
