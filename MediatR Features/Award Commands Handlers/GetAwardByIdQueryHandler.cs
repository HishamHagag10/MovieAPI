using AutoMapper;
using MediatR;
using MovieAPI.MediatR_Features.Award_Commands;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Models.Dtos.Response_Dtos;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.Award_Commands_Handlers
{
    public class GetAwardByIdQueryHandler(AwardRepo _awardRepo,IMapper _mapper) 
        : IRequestHandler<GetAwardByIdQuery, RequestResult<AwardResponseDto>>
    {
        public async Task<RequestResult<AwardResponseDto>> Handle(GetAwardByIdQuery request, CancellationToken cancellationToken)
        {
            var award = await _awardRepo.GetByIdAsync(request.Id);
            if (award == null)
            {
                return RequestResult<AwardResponseDto>.Failure(404,$"No award with Id = {request.Id}");
            }
            var data= _mapper.Map<Award, AwardResponseDto>(award);
            return RequestResult<AwardResponseDto>.Success(data);
        }
    }
}
