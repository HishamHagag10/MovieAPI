using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieAPI.Dtos;
using MovieAPI.Helper;
using MovieAPI.models;
using MovieAPI.Repository;
using NetTopologySuite.Utilities;

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
        public async Task<IActionResult> AddActor([FromBody]AddActorDto dto)
        {

            var IncorrectId = await CheckAwardsAsync(dto.Awards);
            await Console.Out.WriteLineAsync($"{IncorrectId.Item1} {IncorrectId.Item2}");

            if (IncorrectId.Item1 == 1)
                return BadRequest($"You add the Award with Id {IncorrectId.Item2} and year more than once ,Please Don't Duplicate Data");
            else if (IncorrectId.Item1 == 2)
                return NotFound($"No Award With Id ={IncorrectId.Item2}");

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
        public async Task<IActionResult> UpdateActor(int id, [FromBody] UpdateActorDto dto)
        {
            var actor = await _unitOfWork.Actors.GetByIdAsync(id);
            if (actor is null) return NotFound($"NO Actor With id={id}");

            var IncorrectId = await CheckAwardsAsync(dto.Awards);
            if (IncorrectId.Item1 == 1)
                return BadRequest($"You add the Award same Id {IncorrectId.Item2} and year more than once ,Please Don't Duplicate Data");
            else if (IncorrectId.Item1 == 2)
                return NotFound($"No Award With Id ={IncorrectId.Item2}");

            actor = _mapper.Map<UpdateActorDto, Actor>(dto, actor);
            _unitOfWork.Actors.Update(actor);

            var awards = dto.Awards?.
                Select(x => new ActorAwards { ActorId = id, AwardId = x.AwardId, YearOfHonor = x.YearOfHonor });
            if (awards is not null)
                await _unitOfWork.ActorAwards.UpdateOrAddRangeAsync(awards);

            _unitOfWork.SaveChanges();
            return Ok(_mapper.Map<Actor, ReturnActorDto>(actor));
        }
        
        private async Task<(int,int)> CheckAwardsAsync(List<ActorAwardsDto>?awards)
        {
            if (awards is null || !awards.Any()) return (0, 0);
            var set = new HashSet<(int id, int year)>();
            foreach (var award in awards)
            {
                if (!set.Add((award.AwardId, award.YearOfHonor)))
                {
                    return (1,award.AwardId);
                }
                
            }
            var awardsIds = awards.Select(x => x.AwardId).ToHashSet();
            var existAwardsIds = (await _unitOfWork.Awards
                .FindAsync(x => awardsIds.Contains(x.Id), x => x.Id)).ToHashSet();

            if (awardsIds.Count != existAwardsIds.Count)
                return (2,awardsIds.FirstOrDefault(x => !existAwardsIds.Contains(x)));

            return (0,0);
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
