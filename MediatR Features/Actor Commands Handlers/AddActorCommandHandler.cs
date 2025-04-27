using AutoMapper;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.Actor_Commands_Handlers
{
    public class AddActorCommandHandler(ActorRepo _actorRepo,IMapper _mapper) 
        : IRequestHandler<AddActorCommand, RequestResult<ActorResponseDto>>
    {
        public async Task<RequestResult<ActorResponseDto>> Handle(AddActorCommand request, CancellationToken cancellationToken)
        {
            var actor = _mapper.Map<AddActorCommand,Actor>(request);

            var validation =await _actorRepo.ValidateActorAsync(actor);
            if (!validation.IsCorrect)
            {
                return RequestResult<ActorResponseDto>
                    .Failure(400,validation.ErrorMessage!);
            }
            await _actorRepo.AddAsync(actor);
            await _actorRepo.SaveChangesAsync();
            var data = _mapper.Map<Actor, ActorResponseDto>(actor);

            return RequestResult<ActorResponseDto>.Success(data);
        }
    }
}
