using MediatR;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.models.Dtos.Response_Dtos;
using MovieAPI.models;
using AutoMapper;
using MovieAPI.Repository;
using MovieAPI.MediatR_Features.Genre_Commands;
using MovieAPI.Models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Actor_Commands_Handlers
{
    public class UpdateAwardCommandHandler(GenreRepo _genreRepo, IMapper _mapper)
        : IRequestHandler<UpdateGenreCommand, RequestResult<GenreResponseDto>>
    {
        public async Task<RequestResult<GenreResponseDto>> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre =await _genreRepo.GetByIdAsync(request.Id);
            if (genre == null)
            {
                // TODO : Add in Helpers class some thing to handle the messages 
                return RequestResult<GenreResponseDto>.Failure(404,$"No genre with Id={request.Id}");
            }

            genre = _mapper.Map<UpdateGenreCommand, Genre>(request,genre);
            
            await _genreRepo.SaveChangesAsync();

            var data= _mapper.Map<Genre, GenreResponseDto>(genre);
            return RequestResult<GenreResponseDto>.Success(data);
        }
    }
}