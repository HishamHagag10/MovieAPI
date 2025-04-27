using AutoMapper;
using MediatR;
using MovieAPI.MediatR_Features.Award_Commands;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.models;
using MovieAPI.Repository;
using MovieAPI.Models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Award_Commands_Handlers
{
    public class DeleteAwardCommandHandler(AwardRepo _awardRepo, IMapper _mapper) 
        : IRequestHandler<DeleteAwardCommand,RequestResult<AwardResponseDto>>
    {
        public async Task<RequestResult<AwardResponseDto>> Handle(DeleteAwardCommand request, CancellationToken cancellationToken)
        {
            var award = await _awardRepo.GetByIdAsync(request.Id);
            if (award == null)
            {
                return RequestResult <AwardResponseDto>.Failure(404,$"No Award with Id={request.Id}");
            }
            _awardRepo.Delete(award);
            await _awardRepo.SaveChangesAsync();
            var data=_mapper.Map<Award, AwardResponseDto>(award);
            return RequestResult<AwardResponseDto>.Success(data);
        }
    }
}
