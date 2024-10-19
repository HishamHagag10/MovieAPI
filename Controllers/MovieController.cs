using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieAPI.Dtos;
using MovieAPI.Helper;
using MovieAPI.models;
using MovieAPI.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MovieController : ControllerBase
    {
        //private readonly IRepository<Movie> _unitOfWork.Movies;
        //private readonly IRepository<Genre> _unitOfWork.Genres;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOptions<AttachmentOption> _attachOptions;
        public MovieController(IUnitOfWork unitOfWork, IMapper mapper,IOptions<AttachmentOption> attachOptions)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachOptions = attachOptions;
        }

        [HttpGet(template: "GetAllMovie")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMovieAsync([FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10)
        {
            var movies = await _unitOfWork.Movies.FindAsync(x=>true,PageIndex,PageSize,new[] { "Genre" });
            var response =new PagenatedResponse<ReturnMovieDto>
            {
                Data = _mapper.Map<IEnumerable<Movie>, IEnumerable<ReturnMovieDto>>(movies.Data),
                PageIndex = PageIndex,
                PageSize = PageSize,
                TotalPages= movies.TotalPages
            };
            return Ok(response);
        }

        [HttpGet(template: "GetMovie/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMovieAsync(int id)
        {
            var movie = await _unitOfWork.Movies.FirstAsync(x => x.Id == id, new[] { "Genre" });
            if (movie is null)
                return NotFound($"No Movie With ID ={id}");

            return Ok(_mapper.Map<Movie, ReturnMovieDto>(movie));
        }


        [HttpPost(template: "AddMovie")]
        public async Task<IActionResult> AddMovieAsync([FromForm] AddMovieDto dto)
        {
            //_dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Movies', RESEED, 0)");


            if (dto.Poster.Length > _attachOptions.Value.MaxSizeInMegaByte * 1024 * 1024)
                return BadRequest($"Max allowd Length is For poster is {_attachOptions.Value.MaxSizeInMegaByte}MB");

            if (!_attachOptions.Value.AllowedExtentions.Split(';').Contains(Path.GetExtension(dto.Poster.FileName.ToLower())))
                return BadRequest($"Only {_attachOptions.Value.AllowedExtentions} Extention are allowed");

            var genre = await _unitOfWork.Genres.GetByIdAsync(dto.GenreId);
            if (genre is null) 
                return NotFound($"No genre With ID ={dto.GenreId}");

            foreach (var actor in dto.Actors)
            {
                if (!(await _unitOfWork.Actors.AnyAsync(x=>x.Id==actor.actorId)))
                    return NotFound($"NO Actor With Id = {actor.actorId}");
            }

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            var movie=_mapper.Map<AddMovieDto,Movie>(dto);
            movie.Poster = dataStream.ToArray();
            movie.Genre = genre;
            
            movie = await _unitOfWork.Movies.AddAsync(movie);
            _unitOfWork.SaveChanges();

            var actors = dto.Actors
                .Select(x=>new MoviesActors { ActorId=x.actorId,MovieId=movie.Id,Salary=x.Salary}); 
           
            await _unitOfWork.MovieActors.AddRangeAsync(actors);

            _unitOfWork.SaveChanges();
            return Ok(_mapper.Map<Movie, ReturnMovieDto>(movie));
        }

        [HttpPut(template: "UpdateMovie/{id}")]
        public async Task<IActionResult> UpdateMovieAsync(int id, [FromForm] UpdateMovieDto dto)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie is null) return NotFound($"No Movie With ID={id}");

            if (dto.GenreId.HasValue && !(await _unitOfWork.Genres.AnyAsync(x=>x.Id==dto.GenreId)))
                return NotFound($"No genre With ID ={dto.GenreId}");

            if (dto.Actors is not null)
            {
                foreach (var actor in dto.Actors)
                {
                    if (!(await _unitOfWork.Actors.AnyAsync(x => x.Id == actor.actorId)))
                        return NotFound($"NO Actor With Id = {actor.actorId}");
                }
            }
            movie = _mapper.Map<UpdateMovieDto, Movie>(dto, movie);
            if (dto.Poster != null)
            {
                if (dto.Poster.Length > _attachOptions.Value.MaxSizeInMegaByte*1024*1024)
                    return BadRequest($"Max allowd Length is For poster is {_attachOptions.Value.MaxSizeInMegaByte}MB");
                if (!_attachOptions.Value.AllowedExtentions.Split(';').Contains(Path.GetExtension(dto.Poster.FileName.ToLower())))
                    return BadRequest($"Only {_attachOptions.Value.AllowedExtentions} Extention are allowed");
                var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }

            _unitOfWork.Movies.Update(movie);

            var actors = dto.Actors?
               .Select(x => new MoviesActors { ActorId = x.actorId, MovieId = movie.Id, Salary = x.Salary });
           
            if (actors is not null && actors.Any())
                await _unitOfWork.MovieActors.UpdateOrAddRangeAsync(actors);

            _unitOfWork.SaveChanges();
            //return Ok(dto.Actors);
            return Ok(_mapper.Map<Movie, ReturnMovieDto>(movie));
        }

        [HttpDelete(template: "DeleteMovie/{id}")]
        public async Task<IActionResult> DeleteMovieAsync(int id)
        {
            var movie = await _unitOfWork.Movies.FirstAsync(x => x.Id == id, new[] { "Genre" });
            if (movie is null)  
                return NotFound($"No Movie With ID={id}");
            _unitOfWork.Movies.Delete(movie);
            _unitOfWork.SaveChanges();
            return Ok(_mapper.Map<Movie, ReturnMovieDto>(movie));
        }
    }
}
