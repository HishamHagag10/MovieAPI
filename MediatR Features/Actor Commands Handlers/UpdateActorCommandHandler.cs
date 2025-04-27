using MediatR;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.models;
using AutoMapper;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.Actor_Commands_Handlers
{
    public class UpdateActorCommandHandler(ActorRepo _actorRepo, IMapper _mapper) 
        : IRequestHandler<UpdateActorCommand, RequestResult<ActorResponseDto>>
    {
        public async Task<RequestResult<ActorResponseDto>> Handle(UpdateActorCommand request, CancellationToken cancellationToken)
        {
            var actor =await _actorRepo.GetByIdAsync(request.Id);
            if (actor == null)
            {
                return RequestResult<ActorResponseDto>.Failure(404,$"No actor with Id={request.Id}");
            }
            
            actor = _mapper.Map<UpdateActorCommand, Actor>(request,actor);
            
            var validation = await _actorRepo.ValidateActorAsync(actor);
            if (!validation.IsCorrect)
            {
                return RequestResult<ActorResponseDto>.Failure(400,validation.ErrorMessage!);
            }
            await _actorRepo.SaveChangesAsync();

            var data = _mapper.Map<Actor, ActorResponseDto>(actor);
            return RequestResult<ActorResponseDto>.Success(data);
        }
    }
}