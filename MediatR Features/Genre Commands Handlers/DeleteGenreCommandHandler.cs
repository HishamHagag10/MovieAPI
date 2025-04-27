using AutoMapper;
using MediatR;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.models;
using MovieAPI.Repository;
using MovieAPI.MediatR_Features.Genre_Commands;
using MovieAPI.Models.Dtos.Response_Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Azure.Core;

namespace MovieAPI.MediatR_Features.Genre_Commands_Handlers
{
    public class DeleteGenreCommandHandler(GenreRepo _genreRepo,IMapper _mapper)
        : IRequestHandler<DeleteGenreCommand, RequestResult<GenreResponseDto>?>
    {
        public async Task<RequestResult<GenreResponseDto>> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _genreRepo.GetByIdAsync(request.Id);
            if (genre == null)
            {
                return RequestResult<GenreResponseDto>.Failure(404,"No Genre With this Id");
            }
            _genreRepo.Delete(genre);
            await _genreRepo.SaveChangesAsync();
            var data = _mapper.Map<Genre, GenreResponseDto>(genre);
            return RequestResult<GenreResponseDto>.Success(data);
        }
    }
}
