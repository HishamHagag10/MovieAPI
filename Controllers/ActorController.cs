
namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class ActorController : ControllerBase
    {
        private readonly ISender _sender;
        public ActorController(ISender sender)
        {
            _sender = sender;
        }
        [HttpGet("All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActors([FromQuery]GetActorsQuery request)
        {
            var response = await _sender.Send(request);
            if (!response.IsSuccess)
            {
                return NotFound(Helpers.ErrorResponse(404,response.ErrorMessage));
            }
            return Ok(response.Data);
        }

        [HttpGet("Get/{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActorById([FromRoute]GetActorByIdQuery request)
        {
            var response = await _sender.Send(request);
            if (!response.IsSuccess)
            {
                return NotFound(Helpers.ErrorResponse(404,response.ErrorMessage));
            }
            return Ok(response.Data);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddActorAsync([FromBody] AddActorCommand command)
        {
            var response = await _sender.Send(command);
            if (!response.IsSuccess)
                return BadRequest(Helpers.ErrorResponse(400, response.ErrorMessage));
            return Ok(response.Data);
        }

        [HttpPut("Update/{Id}")]
        public async Task<IActionResult> UpdateActor(int Id, [FromBody] UpdateActorCommand request)
        {
            request.Id = Id;
            var response =await _sender.Send(request);
            if (!response.IsSuccess)
            {
                return StatusCode(response.StatusCode,Helpers.ErrorResponse(404, response.ErrorMessage));
            }
            return Ok(response.Data);
        }
        
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> DeleteActor(DeleteActorCommand request)
        {
            var response = await _sender.Send(request);
            if (!response.IsSuccess)
                return NotFound(Helpers.ErrorResponse(404,response.ErrorMessage));
            return Ok(response.Data);
        }
    }
}
