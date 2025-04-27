using AutoMapper;
using MediatR;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.Actor_Commands_Handlers
{
    public class GetActorByIdQueryHandler(ActorRepo _actorRepo,IMapper _mapper) 
        : IRequestHandler<GetActorByIdQuery, RequestResult<ActorResponseDto>>
    {
        public async Task<RequestResult<ActorResponseDto>> Handle(GetActorByIdQuery request, CancellationToken cancellationToken)
        {
            var actor = await _actorRepo.GetByIdAsync(request.Id);
            if (actor == null)
            {
                return RequestResult<ActorResponseDto>.Failure(404,$"No actor with Id={request.Id}");
            }
            var data= _mapper.Map<Actor, ActorResponseDto>(actor);
            return RequestResult<ActorResponseDto>.Success(data);
        }
    }
}
