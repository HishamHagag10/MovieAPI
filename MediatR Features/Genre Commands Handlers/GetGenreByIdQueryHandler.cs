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
    public class GetActorByIdQueryHandler(GenreRepo _genreRepo, IMapper _mapper)
        : IRequestHandler<GetGenreByIdQuery, RequestResult<GenreResponseDto>>
    {
        public async Task<RequestResult<GenreResponseDto>> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            var genre = await _genreRepo.GetByIdAsync(request.Id);
            if (genre == null)
            {
                return RequestResult<GenreResponseDto>.Failure(404,"No Genre with this Id");
            }
            var data= _mapper.Map<Genre, GenreResponseDto>(genre);
            return RequestResult<GenreResponseDto>.Success(data);
        }
    }
}
