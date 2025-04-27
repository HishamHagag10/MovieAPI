using AutoMapper;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.MediatR_Features.Review_Commands;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.Review_Commands_Handlers
{
    public class GetReviewsQueryHandler(ReviewRepo _reviewRepo, PagenatedMapper _pagenatedMapper) 
        : IRequestHandler<GetReviewsQuery, RequestResult<PagenatedResponse<ReviewResponseDto>>>
    {
        public async Task<RequestResult<PagenatedResponse<ReviewResponseDto>>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
        {
            var repoResponse = await _reviewRepo.GetReviewsAsync(request.PageIndex,
                request.PageSize,request.OrderBy,request.OrderType,request.MovieId,
                request.UserId);
            if (repoResponse == null || repoResponse.Count==0)
            {
                return RequestResult<PagenatedResponse<ReviewResponseDto>>
                    .Failure(400,"No data matches");
            }
            
            var data= _pagenatedMapper.Map<Review, ReviewResponseDto>(repoResponse.Data!, request.PageIndex
                ,request.PageSize, repoResponse.Count);
            return RequestResult<PagenatedResponse<ReviewResponseDto>>.Success(data);
        }
    }
}
