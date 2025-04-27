using AutoMapper;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.MediatR_Features.Award_Commands;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Models.Dtos.Response_Dtos;
using MovieAPI.Repository;
using MovieAPI.Repository.UnitOfWork;

namespace MovieAPI.MediatR_Features.Award_Commands_Handlers
{
    public class GetActorsAwardsQueryHandler(IUnitOfWork _unitOfWork, PagenatedMapper _pagenatedMapper) 
        : IRequestHandler<GetActorsAwardsQuery, RequestResult<PagenatedResponse<ActorAwardResponseDto>>>
    {
        
        public async Task<RequestResult<PagenatedResponse<ActorAwardResponseDto>>> Handle(GetActorsAwardsQuery request, 
            CancellationToken cancellationToken)
        {
            var repoResponse = await _unitOfWork.ActorAwardRepo.GetActorsAwards(request.PageIndex,
                request.PageSize,request.OrderType,request.ActorId,request.YearOfHonor);

            if (repoResponse == null || repoResponse.Count == 0)
                return RequestResult <PagenatedResponse<ActorAwardResponseDto>>.Failure(400,"No Data matches");

            var data= _pagenatedMapper.Map<ActorAward,ActorAwardResponseDto>(repoResponse.Data,request.PageIndex,request.PageSize,repoResponse.Count);
            return RequestResult<PagenatedResponse<ActorAwardResponseDto>>.Success(data);
        }
    }
}
