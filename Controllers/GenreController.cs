using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieAPI.Dtos;
using MovieAPI.Helper;
using MovieAPI.models;
using MovieAPI.Repository;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class GenreController : ControllerBase
    {
        //private readonly IRepository<Movie> _movieRepository;
        //private readonly IRepository<Genre> _unitOfWork.Genres;
        private readonly IUnitOfWork _unitOfWork;

        public GenreController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        /*public GenreController(IRepository<Genre> repository, IRepository<Movie> movieRepository)
        {
            this._unitOfWork.Genres = repository;
            _movieRepository = movieRepository;
        }*/

        [HttpGet]
        [Route("GetAllGenres")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10)
        {
            var genres = await _unitOfWork.Genres.GetAllAsync(PageIndex,PageSize);
            var response = new PagenatedResponse<string>
            {
                Data = genres.Data.Select(x => x.Name),
                PageIndex = PageIndex,
                PageSize = PageSize,
                TotalPages = genres.TotalPages,
            };
            return Ok(genres);
        }
        [HttpGet("GetGenre/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(id);
            if(genre == null) return NotFound($"No Genre With Id {id}"); 
            return Ok(genre);
        }

        [HttpPost]
        [Route("AddNewGenre")]
        public async Task<IActionResult> AddGenre(AddGenreDto genreDto)
        {
            var genre = new Genre { Name = genreDto.Name };
            genre = await _unitOfWork.Genres.AddAsync(genre);
            _unitOfWork.SaveChanges();
            return Ok(genre);
        }

        [HttpPut(template:"UpdateGenre/{id}")]
        public async Task<IActionResult> UpdateGenre(int id,[FromBody]AddGenreDto dto)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(id);
            if(genre is null)
            {
                return NotFound($"No Genre With ID = {id}");
            }
            genre.Name = dto.Name;
            genre = _unitOfWork.Genres.Update(genre);
            _unitOfWork.SaveChanges();
            return Ok(genre);
        }


        [HttpDelete(template:"DeleteGenre/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(id); 
            if(genre is null)
            {
                return NotFound($"No Genre With ID = {id}");
            }
            if (await _unitOfWork.Movies.AnyAsync(x=>x.GenreId==id))
            {
                return BadRequest($"Delete Movies with genre Id = {id} Before Delete it!");
            }
            genre = _unitOfWork.Genres.Delete(genre);
            _unitOfWork.SaveChanges();
            return Ok(genre);
        }
    }
}
