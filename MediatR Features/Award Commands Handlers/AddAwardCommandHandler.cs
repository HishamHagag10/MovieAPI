using AutoMapper;
using MediatR;
using MovieAPI.MediatR_Features.Award_Commands;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Models.Dtos.Response_Dtos;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.Award_Commands_Handlers
{
    public class AddAwardCommandHandler(AwardRepo _awardRepo,IMapper _mapper) 
        : IRequestHandler<AddAwardCommand, RequestResult<AwardResponseDto>>
    {
        public async Task<RequestResult<AwardResponseDto>> Handle(AddAwardCommand request, CancellationToken cancellationToken)
        {
            var award = _mapper.Map<AddAwardCommand,Award>(request);
            
            await _awardRepo.AddAsync(award);
            await _awardRepo.SaveChangesAsync();
            
            var data= _mapper.Map<Award,AwardResponseDto>(award);
            return RequestResult <AwardResponseDto>.Success(data);
        }
    }
}
