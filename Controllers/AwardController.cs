using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Dtos;
using MovieAPI.Helper;
using MovieAPI.models;
using MovieAPI.Repository;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AwardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AwardController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        /*public AwardController(IRepository<Award> repository, IRepository<Movie> movieRepository)
        {
            this._unitOfWork.Awards = repository;
            _movieRepository = movieRepository;
        }*/

        [HttpGet]
        [Route("GetAllAwards")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10)
        {
            var awards = await _unitOfWork.Awards.GetAllAsync(PageIndex,PageSize);
            var response = new PagenatedResponse<string>
            {
                Data = awards.Data.Select(x=>x.Name),
                PageIndex = PageIndex,
                PageSize = PageSize,
                TotalPages = awards.TotalPages,
            };
            return Ok(response);
        }
        [HttpGet("GetAward/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var award = await _unitOfWork.Awards.GetByIdAsync(id);
            if (award == null) return NotFound($"No Award With Id {id}");
            return Ok(new { award.Id, award.Name });
        }

        [HttpPost]
        [Route("AddNewAward")]
        public async Task<IActionResult> AddAward(AddAwardDto AwardDto)
        {
            var award = new Award { Name = AwardDto.Name };
            award = await _unitOfWork.Awards.AddAsync(award);
            _unitOfWork.SaveChanges();
            return Ok(new { award.Id, award.Name });
        }

        [HttpPut(template: "UpdateAward/{id}")]
        public async Task<IActionResult> UpdateAward(int id, [FromBody] AddAwardDto dto)
        {
            var award = await _unitOfWork.Awards.GetByIdAsync(id);
            if (award is null)
            {
                return NotFound($"No Award With Id = {id}");
            }
            award.Name = dto.Name;
            award = _unitOfWork.Awards.Update(award);
            _unitOfWork.SaveChanges();
            return Ok(new {award.Id,award.Name});
        }


        [HttpDelete(template: "DeleteAward/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var award = await _unitOfWork.Awards.GetByIdAsync(id);
            if (award is null)
            {
                return NotFound($"No Award With ID = {id}");
            }
            award = _unitOfWork.Awards.Delete(award);
            _unitOfWork.SaveChanges();
            return Ok(new { award.Id, award.Name });
        }
    }
}
