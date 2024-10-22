using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class ActorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PagenatedMapper _pagenatedmapper;
        public ActorController(IUnitOfWork unitOfWork, IMapper mapper, PagenatedMapper pmapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _pagenatedmapper = pmapper;
        }
        [HttpGet("AllActors")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActors([FromQuery] int PageIndex=1 ,[FromQuery] int PageSize=10)
        {
            var actors = await _unitOfWork.Actors.GetAllAsync(PageIndex,PageSize);
            
            var response = _pagenatedmapper.Map<Actor, ReturnActorDto>(actors);
            
            return Ok(response);
        }

        [HttpGet("GetActorById/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActorById(int id)
        {
            var actor = await _unitOfWork.Actors.GetByIdAsync(id);
            if (actor is null) return NotFound($"NO Actor with Id = {id}");
            return Ok(_mapper.Map<Actor,ReturnActorDto>(actor));
        }

        [HttpPost("AddActor")]
        public async Task<IActionResult> AddActor([FromForm]AddActorDto dto)
        {
            if (dto.Awards is not null)
            foreach (var award in dto.Awards)
            {
                if (!(await _unitOfWork.Awards.AnyAsync(x=>x.Id==award.AwardId))) 
                    return NotFound($"NO Award With Id = {award.AwardId}");
            }
            
            var actor = _mapper.Map<AddActorDto,Actor>(dto);
            actor = await _unitOfWork.Actors.AddAsync(actor);

            _unitOfWork.SaveChanges();

            var awards = dto.Awards?.
               Select(x => new ActorAwards { ActorId = actor.Id, AwardId = x.AwardId, YearOfHonor = x.YearOfHonor });
            if (awards is not null)
                await _unitOfWork.ActorAwards.AddRangeAsync(awards);

            _unitOfWork.SaveChanges();
            return Ok(_mapper.Map<Actor, ReturnActorDto>(actor));
        }

        [HttpPut("UpdateActor/{id}")]
        public async Task<IActionResult> UpdateActor(int id,[FromForm] UpdateActorDto dto)
        {
            var actor = await _unitOfWork.Actors.GetByIdAsync(id);
            if (actor is null) return NotFound($"NO Actor With id={id}");
            if (dto.Awards is not null)
            {
                foreach (var award in dto.Awards)
                {
                    if (!(await _unitOfWork.Awards.AnyAsync(x => x.Id == award.AwardId)))
                        return NotFound($"NO Award With Id = {award.AwardId}");
                }
            }
            actor = _mapper.Map<UpdateActorDto, Actor>(dto,actor);
            _unitOfWork.Actors.Update(actor);
            
            var awards = dto.Awards?.
                Select(x => new ActorAwards { ActorId = id, AwardId = x.AwardId, YearOfHonor = x.YearOfHonor });
            if (awards is not null)
                await _unitOfWork.ActorAwards.UpdateOrAddRangeAsync(awards);
            
            _unitOfWork.SaveChanges();
            return Ok(_mapper.Map<Actor, ReturnActorDto>(actor));
        }

        [HttpDelete("DeleteActor/{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            var actor = await _unitOfWork.Actors.GetByIdAsync(id);
            if(actor is null)return NotFound($"NO actor With Id = {id}");
            actor = _unitOfWork.Actors.Delete(actor);
            _unitOfWork.SaveChanges();
            return Ok(actor);
        }
    }
}
