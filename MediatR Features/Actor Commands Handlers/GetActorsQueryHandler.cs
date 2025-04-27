using AutoMapper;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.Actor_Commands_Handlers
{
    public class GetActorsQueryHandler(ActorRepo _actorRepo, PagenatedMapper _pagenatedMapper) 
        : IRequestHandler<GetActorsQuery, RequestResult<PagenatedResponse<ActorResponseDto>>>
    {
        public async Task<RequestResult<PagenatedResponse<ActorResponseDto>>> Handle(GetActorsQuery request, CancellationToken cancellationToken)
        {
            var repoResponse = await _actorRepo.GetActorsAsync(request.PageIndex,
                request.PageSize,request.OrderBy,request.OrderType,request.MovieId,
                request.AwardId);
            if (repoResponse == null || repoResponse.Count==0)
            {
                return RequestResult<PagenatedResponse<ActorResponseDto>>.Failure(400,"No data matches");
            }
            
            var data= _pagenatedMapper.Map<Actor,ActorResponseDto>(repoResponse.Data, request.PageIndex
                ,request.PageSize, repoResponse.Count);
            return RequestResult<PagenatedResponse<ActorResponseDto>>.Success(data);
        }
    }
}
