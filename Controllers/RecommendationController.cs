using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Dtos;
using MovieAPI.Helper;
using MovieAPI.models;
using MovieAPI.Repository;
using System.Security.Claims;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin,User")]
    public class RecommendationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PagenatedMapper _pagenatedMapper;
        public RecommendationController(IUnitOfWork unitOfWork, IMapper mapper, PagenatedMapper pagenatedMapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _pagenatedMapper = pagenatedMapper;
        }
        [HttpGet("RecommendedMoviesBasedOnGenre")]
        public async Task<IActionResult> RecommendedMoviesBasedOnGenre([FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10) 
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized();
            }
            if (!(await _unitOfWork.Users.AnyAsync(x => x.Id == userId)))
            {
                return NotFound();
            }
            var movies = await _unitOfWork.Recommendation.
                RecommendationsMoviesbasedOnGenreAsync(userId,PageIndex,PageSize);
            var response = _pagenatedMapper.Map<Movie, ReturnMovieDto>(movies);
            
            return Ok(response);
        }
        [HttpGet("RecommendedMoviesBasedOnActor")]
        public async Task<IActionResult> RecommendedMoviesBasedOnActors([FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized();
            }
            if (!(await _unitOfWork.Users.AnyAsync(x => x.Id == userId)))
            {
                return NotFound();
            }
            var movies = await _unitOfWork.Recommendation.
                RecommendationsMoviesbasedOnActorAsync(userId, PageIndex, PageSize);

            var response = _pagenatedMapper.Map<Movie, ReturnMovieDto>(movies);
            
            return Ok(response);
        }
    }
}
