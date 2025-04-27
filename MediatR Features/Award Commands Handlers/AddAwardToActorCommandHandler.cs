using AutoMapper;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.MediatR_Features.Award_Commands;
using MovieAPI.models;
using MovieAPI.Models.Dtos.Response_Dtos;
using MovieAPI.Repository;
using MovieAPI.Repository.UnitOfWork;

namespace MovieAPI.MediatR_Features.Award_Commands_Handlers
{
    public class AddAwardToActorCommandHandler(IUnitOfWork _unitOfWork,IMapper _mapper)
        : IRequestHandler<AddAwardToActorCommand, RequestResult<ActorAwardResponseDto>>
    {
        public async Task<RequestResult<ActorAwardResponseDto>> Handle(AddAwardToActorCommand request, CancellationToken cancellationToken)
        {
            var actor = await _unitOfWork.ActorRepo.GetByIdAsync(request.ActorId);
            if(actor==null)
                return RequestResult<ActorAwardResponseDto>
                    .Failure(400,$"No Actor with id={request.ActorId}");
            
            var award = await _unitOfWork.AwardRepo.GetByIdAsync(request.AwardId);
            if (award==null)
                return RequestResult<ActorAwardResponseDto>
                    .Failure(400, $"No Award with id={request.ActorId}");

            var validation = await _unitOfWork.ActorAwardRepo.CheckYearOfHonorOfAward(request.AwardId,request.YearOfHonor);
            if (!validation.IsCorrect)
            {
                return RequestResult<ActorAwardResponseDto>.Failure(404,
                    validation.ErrorMessage);
            }

            var actorAward = _mapper.Map<AddAwardToActorCommand,ActorAward>(request);
            await _unitOfWork.ActorAwardRepo.AddAsync(actorAward);
            await _unitOfWork.ActorAwardRepo.SaveChangesAsync();
            actorAward.Award = award;
            actorAward.Actor = actor;
            var data= _mapper.Map<ActorAward, ActorAwardResponseDto>(actorAward);
            return RequestResult <ActorAwardResponseDto>.Success(data);
        }
    }
}
