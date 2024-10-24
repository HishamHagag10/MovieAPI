using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Dtos;
using MovieAPI.models;
using MovieAPI.Repository;
using System.Security.Claims;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class ReviewController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet("GetReview/{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await _unitOfWork.Reviews.FirstAsync(x=>x.Id==id,
                x=>new ReturnReviewDto{
                    Id=x.Id,Description = x.Description,
                    Rate = x.Rate,
                    UserName=x.User.Name,
                    MovieTitle=x.Movie.Title
                },
                q=>q.Include(x=>x.Movie).Include(x=>x.User));
            if (review == null) 
            {
                return NotFound($"NO Review With Id = {id}");
            }
            return Ok(review);
        }

        // TODO: try to optimize.
        [HttpPost("AddReview/")]
        public async Task<IActionResult> AddReview(AddReviewDto dto)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized();
            }
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user is null) return NotFound();
            
            var movie = await _unitOfWork.Movies.GetByIdAsync(dto.MovieId);
            if(movie is null) return NotFound($"NO Movie With Id = {dto.MovieId}");
            
            var review = _mapper.Map<AddReviewDto,Review>(dto);
            review.UserId = userId;
            review = await _unitOfWork.Reviews.AddAsync(review);
            
            movie.SumOfReview += review.Rate;
            movie.NoOfReview++;
            
            _unitOfWork.SaveChanges();
            review.User = user;
            review.Movie = movie;
            return Ok(_mapper.Map<Review,ReturnReviewDto>(review));
        }

        // TODO : try to optimize.
        [HttpPost("UpdateReview/{id}")]
        public async Task<IActionResult> UpdateReview(int id,UpdateReviewDto dto)
        {
            
            var review = await _unitOfWork.Reviews.FirstAsync(x=>x.Id==id,
                q=>q.Include(x=>x.Movie).Include(x=>x.User));
            if (review == null) return NotFound($"NO Review With Id = {id}");
            if (User.FindFirst(ClaimTypes.Role)?.Value == Role.User.ToString())
            {
                if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userid)
                    || userid != review.UserId)
                {
                    return Unauthorized();
                }
            }
            if (dto.Rate.HasValue)
            {
                review.Movie.SumOfReview -= (int)review.Rate;
                review.Movie.SumOfReview += (int)dto.Rate;
            }
            review = _mapper.Map<UpdateReviewDto, Review>(dto,review);
            review = _unitOfWork.Reviews.Update(review);
            _unitOfWork.SaveChanges();
            
            return Ok(_mapper.Map<Review, ReturnReviewDto>(review));
        }

        [HttpDelete("DeleteReview/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _unitOfWork.Reviews.FirstAsync(x => x.Id == id,x=>x.Include(x=>x.Movie).Include(x=>x.User));//new[] { "Movie", "User" });
            if (review == null) return NotFound($"NO Review With Id = {id}");
            if (User.FindFirst(ClaimTypes.Role)?.Value == Role.User.ToString())
            {
                if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userid)
                    || userid != review.UserId)
                {
                    return Unauthorized();
                }
            }
            review.Movie.SumOfReview -= review.Rate;
            review.Movie.NoOfReview--;
            review = _unitOfWork.Reviews.Delete(review);
            _unitOfWork.SaveChanges();
            return Ok(_mapper.Map<Review, ReturnReviewDto>(review));
        }

    }
}
