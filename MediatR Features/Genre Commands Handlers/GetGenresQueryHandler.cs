using AutoMapper;
using MediatR;
using MovieAPI.Helper;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.MediatR_Features.Genre_Commands;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Models.Dtos.Response_Dtos;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.Genre_Commands_Handlers
{
    public class GetGenresQueryHandler(GenreRepo _genreRepo, PagenatedMapper _pagenatedMapper) 
        : IRequestHandler<GetGenresQuery, RequestResult<PagenatedResponse<GenreResponseDto>>>
    {
        public async Task<RequestResult<PagenatedResponse<GenreResponseDto>>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            var repoResponse = await _genreRepo.GetGenresAsync(request.PageIndex,
                request.PageSize, request.OrderBy, request.OrderType);

            if (repoResponse == null || repoResponse.Count == 0)
            {
                return RequestResult<PagenatedResponse<GenreResponseDto>>.Failure(404,"No Genre with this Id");
            }

            var data= _pagenatedMapper.Map<Genre, GenreResponseDto>(repoResponse.Data, 
                request.PageIndex
                , request.PageSize, repoResponse.Count);
            return RequestResult<PagenatedResponse<GenreResponseDto>>.Success(data);
        }
    }
}
