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
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PagenatedMapper _pagenatedMapper;
        private readonly IOptions<AttachmentOption> _attachOptions;
        public MovieController(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AttachmentOption> attachOptions, PagenatedMapper pagenatedMapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachOptions = attachOptions;
            _pagenatedMapper = pagenatedMapper;
        }

        [HttpGet(template: "GetAllMovie")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMovieAsync([FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10)
        {
            var movies = await _unitOfWork.Movies.FindPagenatedAsync(x=>true,PageIndex,PageSize,q=>q.Include(x=>x.Genre));
            var response =_pagenatedMapper.Map<Movie,ReturnMovieDto>(movies);
            
            return Ok(response);
        }

        [HttpGet(template: "GetMovie/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _unitOfWork.Movies.FirstAsync(x => x.Id == id,q=>q.Include(x=>x.Genre));
            if (movie is null)
                return NotFound($"No Movie With ID ={id}");

            return Ok(_mapper.Map<Movie, ReturnMovieDto>(movie));
        }


        [HttpPost(template: "AddMovie")]
        [Authorize(Roles ="Admin")]
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

            var IncorrectActor = await CheckActorsAsync(dto.Actors);
            if (IncorrectActor.Item1 == 1)
                return BadRequest($"You Added the Actor With Id {IncorrectActor.Item2} more than once, Please Don't Duplicate Data");
            else if (IncorrectActor.Item1 == 2)
                return NotFound($"NO Actor With Id = {IncorrectActor.Item2}");


            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            var movie = _mapper.Map<AddMovieDto, Movie>(dto);
            movie.Poster = dataStream.ToArray();
            movie.Genre = genre;

            movie = await _unitOfWork.Movies.AddAsync(movie);
            _unitOfWork.SaveChanges();

            var actors = dto.Actors
                .Select(x => new MoviesActors
                {
                    ActorId = x.actorId,
                    MovieId = movie.Id
                ,
                    Salary = x.Salary
                });

            await _unitOfWork.MovieActors.AddRangeAsync(actors);

            _unitOfWork.SaveChanges();
            return Ok(_mapper.Map<Movie, ReturnMovieDto>(movie));
        }

        [HttpPut(template: "UpdateMovie/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateMovieAsync(int id, [FromForm] UpdateMovieDto dto)
        {
            var movie = await _unitOfWork.Movies.FirstAsync(x=>x.Id==id,q=>q.Include(x=>x.Genre));
            if (movie is null) return NotFound($"No Movie With ID={id}");

            if (dto.GenreId.HasValue && !(await _unitOfWork.Genres.AnyAsync(x => x.Id == dto.GenreId)))
                return NotFound($"No genre With ID ={dto.GenreId}");

            var IncorrectActor = await CheckActorsAsync(dto.Actors);
            if (IncorrectActor.Item1 == 1)
                return BadRequest($"You Added the Actor With Id {IncorrectActor.Item2} more than once, Please Don't Duplicate Data");
            else if (IncorrectActor.Item1 == 2)
                return NotFound($"NO Actor With Id = {IncorrectActor.Item2}");


            movie = _mapper.Map<UpdateMovieDto, Movie>(dto, movie);
            if (dto.Poster != null)
            {
                if (dto.Poster.Length > _attachOptions.Value.MaxSizeInMegaByte * 1024 * 1024)
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
        private async Task<(int, int)> CheckActorsAsync(IEnumerable<MoviesActorsDto>? actors)
        {
            if (actors is null || !actors.Any()) return (0, 0);

            var actorsIds = new HashSet<int>();
            foreach (var actor in actors)
            {
                if (!actorsIds.Add(actor.actorId))
                    return (1, actor.actorId);
            }

            var existAwardsIds = (await _unitOfWork.Actors
                .FindAsync(x => actorsIds.Contains(x.Id), x => x.Id)).ToHashSet();

            if (actorsIds.Count != existAwardsIds.Count)
                return (2, actorsIds.FirstOrDefault(x => !existAwardsIds.Contains(x)));

            return (0, 0);
        }
        [HttpDelete(template: "DeleteMovie/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteMovieAsync(int id)
        {
            var movie = await _unitOfWork.Movies.FirstAsync(x => x.Id == id, q => q.Include(x => x.Genre));
            if (movie is null)  
                return NotFound($"No Movie With Id={id}");
            _unitOfWork.Movies.Delete(movie);
            _unitOfWork.SaveChanges();
            return Ok(_mapper.Map<Movie, ReturnMovieDto>(movie));
        }
        [HttpPost("MovieWatched/{movieId}")]
        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> MovieWatched(int movieId)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized();
            }
            if (!(await _unitOfWork.Users.AnyAsync(x => x.Id == userId)))
            {
                return NotFound();
            }
            if (!(await _unitOfWork.Movies.AnyAsync(x => x.Id == movieId)))
                return NotFound($"No Movie With Id={movieId}");
            var userMovie = new UserMovies {
                MovieId = movieId,
                UserId = userId,
                WatchedAt = DateTime.UtcNow
            };
            await _unitOfWork.UserMovies.AddAsync(userMovie);
            _unitOfWork.SaveChanges();
            return Ok();
        }
    }
}
