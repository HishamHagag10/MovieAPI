using AutoMapper;
using MediatR;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.models;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.Actor_Commands_Handlers
{
    public class DeleteActorCommandHandler(ActorRepo _actorRepo, IMapper _mapper) 
        : IRequestHandler<DeleteActorCommand, RequestResult<ActorResponseDto>>
    {
        public async Task<RequestResult<ActorResponseDto>> Handle(DeleteActorCommand request,
            CancellationToken cancellationToken)
        {
            var actor = await _actorRepo.GetByIdAsync(request.Id);
            if (actor == null)
            {
                return RequestResult<ActorResponseDto>.Failure(404,$"No data with Id={request.Id}");
            }
            _actorRepo.Delete(actor);
            await _actorRepo.SaveChangesAsync();
            var data= _mapper.Map<Actor, ActorResponseDto>(actor);
            return RequestResult<ActorResponseDto>.Success(data);
        }
    }
}
