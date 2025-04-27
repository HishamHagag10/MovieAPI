
namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AwardsController : ControllerBase
    {
        private readonly ISender _sender;

        public AwardsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetAwardsQuery request)
        { 
            var response = await _sender.Send(request);
            if (!response.IsSuccess)
                return NotFound(Helpers.ErrorResponse(response.StatusCode,response.ErrorMessage));
            return Ok(response.Data);
        }
        [HttpGet("Get/{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync([FromRoute] GetAwardByIdQuery request)
        {
            var response = await _sender.Send(request);
            if (!response.IsSuccess)
                return NotFound(Helpers.ErrorResponse(response.StatusCode, response.ErrorMessage));
            return Ok(response.Data);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddAward(AddAwardCommand request)
        {
            var response = await _sender.Send(request);
            if (!response.IsSuccess)
                return BadRequest(Helpers.ErrorResponse(response.StatusCode, response.ErrorMessage));
            
            return Ok(response.Data);
        }

        [HttpPut(template: "Update/{id}")]
        public async Task<IActionResult> UpdateAward(int id, UpdateAwardCommand request)
        {
            request.Id = id;
            var response = await _sender.Send(request);
            if (!response.IsSuccess)
            {
                return StatusCode(response.StatusCode, Helpers.ErrorResponse(404, response.ErrorMessage));
            }
            return Ok(response.Data);
        }


        [HttpPost(template: "{AwardId}/Actor/{ActorId}")]
        public async Task<IActionResult> AddAwardtoActorAsync(int ActorId,int AwardId,AddAwardToActorCommand request)
        {
            request.ActorId = ActorId;
            request.AwardId = AwardId;
            var response = await _sender.Send(request);
            if (!response.IsSuccess)
            {
                return StatusCode(response.StatusCode, Helpers.ErrorResponse(404, response.ErrorMessage));
            }
            return Ok(response.Data);
        }

        [HttpGet(template: "Actors")]
        public async Task<IActionResult> GetAwardOfActorAsync([FromQuery] GetActorsAwardsQuery request)
        {
            var response = await _sender.Send(request);
            
            if (!response.IsSuccess)
                return NotFound(Helpers.ErrorResponse(400,response.ErrorMessage));

            return Ok(response.Data);
        }


        [HttpDelete(template: "Delete/{Id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] DeleteAwardCommand request)
        {
            var response = await _sender.Send(request);

            if (!response.IsSuccess)
                return NotFound(Helpers.ErrorResponse(400, response.ErrorMessage));

            return Ok(response.Data);
        }
    }
}
