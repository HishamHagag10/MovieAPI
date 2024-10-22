using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieAPI.Dtos;
using MovieAPI.Helper;
using MovieAPI.models;
using MovieAPI.Repository;
using System.Security.Claims;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PagenatedMapper _pagenatedMapper;
        public SearchController(IUnitOfWork unitOfWork, IMapper mapper, PagenatedMapper pagenatedMapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _pagenatedMapper = pagenatedMapper;
        }

        [HttpGet(template: "MoviesOfGenre/{genreId}")]
        public async Task<IActionResult> GetMoviesByGenreAsync(int genreId, [FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10)
        {
            if (!(await _unitOfWork.Genres.AnyAsync(x => x.Id == genreId)))
                return NotFound($"No genre With ID ={genreId}");

            var movies = await _unitOfWork.Movies.FindAsync(x => x.GenreId == genreId,PageIndex,PageSize, new[] { "Genre" });
            var response = _pagenatedMapper.Map<Movie, ReturnMovieDto>(movies);
            
            return Ok(response);
        }

        [HttpGet("MoviesOfActor/{actorId}")]
        public async Task<IActionResult> GetMoviesByActor(int actorId, [FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10)
        {
            if (!(await _unitOfWork.Actors.AnyAsync(x=>x.Id==actorId)))
                return NotFound($"NO Actor With id = {actorId}");

            var movies = await _unitOfWork.MovieActors.
                FindAsync(x => x.ActorId == actorId,
                x=>x.Movie
                ,PageIndex, PageSize, new[] { "Movie" });

            var response = _pagenatedMapper.Map<Movie, ReturnMovieDto>(movies);

            return Ok(response);
        }
        [HttpGet("GetMoviesByReleaseyear/{year}")]
        public async Task<IActionResult> GetMoviesByReleaseyear(int year, [FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10)
        {
            if (year > DateTime.Now.Year + 5 || year<1900)
            {
                return BadRequest("Enter valid year");
            }
            var movies = await _unitOfWork.Movies.FindAsync(x => x.year == year, 
                PageIndex, PageSize, new[] {"Genre"} );
            var response = _pagenatedMapper.Map<Movie,ReturnMovieDto>(movies);
            return Ok(response);
        }
        [HttpGet("ReviewsOfMovie/{movieId}")]
        public async Task<IActionResult> GetReviewsOfMovie(int movieId, [FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10)
        {
            if (!(await _unitOfWork.Movies.AnyAsync(x => x.Id == movieId)))
                return NotFound($"NO Movie With this id = {movieId}");
            
            var reviews = await _unitOfWork.Reviews.FindAsync(x => x.MovieId == movieId,
                x => new ReturnReviewDto
                {
                    Id = x.Id,
                    Description = x.Description,
                    Rate = x.Rate,
                    UserName = x.User.Name,
                    MovieTitle = x.Movie.Title
                },PageIndex,PageSize,
                new[] { "Movie", "User" });

            return Ok(reviews);
        }

        [HttpGet("ActorsOfMovie/{movieId}")]
        public async Task<IActionResult> GetActorsOfMovie(int movieId, [FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10)
        {
            if (!(await _unitOfWork.Movies.AnyAsync(x => x.Id == movieId)))
                return NotFound($"NO Movie With this id = {movieId}");

            var actors = await _unitOfWork.MovieActors.FindAsync(x => x.MovieId == movieId,
                x => new { x.ActorId, x.Actor.Name, x.Actor.Email, x.Salary }
                ,PageIndex,PageSize,new[] { "Actor" });

            return Ok(actors);
        }

        [HttpGet("AwardsOfActor/{actorId}")]
        public async Task<IActionResult> AwardsOfActor(int actorId, [FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10)
        {

            if (!(await _unitOfWork.Actors.AnyAsync(x => x.Id == actorId)))
                return NotFound($"NO Actor with id = {actorId}");

            var awards = await _unitOfWork.ActorAwards
                .FindAsync(x => x.ActorId == actorId,
                x => new { x.AwardId, Award = x.Award.Name, x.YearOfHonor },
                PageIndex, PageSize,
                new[] { "Award" });

            return Ok(awards);
        }

        [HttpGet("ActorsGotAward/{awardId}")]
        public async Task<IActionResult> ActorsGotAward(int awardId, [FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10)
        {
            if (!(await _unitOfWork.Awards.AnyAsync(x => x.Id == awardId)))
                return NotFound($"NO Award With Id = {awardId}");

            var actors = await _unitOfWork.ActorAwards
               .FindAsync(x => x.AwardId == awardId,
               x => new { x.ActorId, x.Actor.Name, x.YearOfHonor },
               PageIndex, PageSize,
               new[] { "Actor" });

            return Ok(actors);
        }
        [HttpGet("ReviewsofUser")]
        [Authorize(Roles ="User,Admin")]
        public async Task<IActionResult> ReviewsofUser([FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userid))
                return Unauthorized();
            var Reviews = await _unitOfWork.Reviews.FindAsync(x => x.UserId == userid,
                x=>new ReturnReviewDto
                {
                    Id= x.Id,
                    Description= x.Description,
                    UserName = x.User.Name,
                    MovieTitle=x.Movie.Title,
                    Rate=x.Rate
                },PageIndex,PageSize, new[] {"User","Movie"} );
            return Ok(Reviews);
        }
        [HttpGet("MoviesWatchedByUser")]
        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> MoviesWatchedByUser([FromQuery] int PageIndex=1, [FromQuery] int PageSize = 10)
        {
            if(!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value,out int userId))
            {
                return Unauthorized();
            }
            var movies = await _unitOfWork.UserMovies
                .FindAsync(x => x.UserId==userId,x=>x.Movie,
                PageIndex,PageSize,
                new[] { "Movie" } );

            var response = _pagenatedMapper.Map<Movie, ReturnMovieDto>(movies);

            
            return Ok(response);
        }
        // TODO : Search in all files and use select to optimize performance

    }
}