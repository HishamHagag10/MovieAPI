using AutoMapper;
using Azure.Core;
using MediatR;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.MediatR_Features.Genre_Commands;
using MovieAPI.models;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.Models.Dtos.Response_Dtos;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.Genre_Commands_Handlers
{
    public class AddGenreCommandHandler(GenreRepo _genreRepo,IMapper _mapper) 
        : IRequestHandler<AddGenreCommand, RequestResult<GenreResponseDto>>
    {
        public async Task<RequestResult<GenreResponseDto>> Handle(AddGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = _mapper.Map<AddGenreCommand, Genre>(request);

            await _genreRepo.AddAsync(genre);
            await _genreRepo.SaveChangesAsync();

            var data=_mapper.Map<Genre,GenreResponseDto >(genre);
            return RequestResult<GenreResponseDto>.Success(data);
        }
    }
}
