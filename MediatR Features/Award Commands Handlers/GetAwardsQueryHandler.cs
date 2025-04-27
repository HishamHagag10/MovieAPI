using AutoMapper;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.MediatR_Features.Award_Commands;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Models.Dtos.Response_Dtos;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.Award_Commands_Handlers
{
    public class GetAwardsQueryHandler(AwardRepo _awardRepo, PagenatedMapper _pagenatedMapper) 
        : IRequestHandler<GetAwardsQuery, RequestResult<PagenatedResponse<AwardResponseDto>>>
    {
        public async Task<RequestResult<PagenatedResponse<AwardResponseDto>>> Handle(GetAwardsQuery request, CancellationToken cancellationToken)
        {
            var repoResponse = await _awardRepo.GetAwardsAsync(request.PageIndex,
                request.PageSize,request.OrderType);
            
            if (repoResponse == null || repoResponse.Count == 0)
                return RequestResult<PagenatedResponse<AwardResponseDto>>.Failure(400,"NO Data Matches");

            var data = _pagenatedMapper.Map<Award, AwardResponseDto>(repoResponse.Data, request.PageIndex,request.PageSize,repoResponse.Count);
            return RequestResult <PagenatedResponse<AwardResponseDto>>.Success(data);
        }
    }
}
