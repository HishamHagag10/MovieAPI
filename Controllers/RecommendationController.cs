
namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class RecommendationController : ControllerBase
    {
        private readonly ISender _sender;

        public RecommendationController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("OnGenre")]
        public async Task<IActionResult> RecommendedMoviesBasedOnGenre([FromQuery] RecommendMoviesOnGenreQuery request)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized();
            }
            request.UserId = userId;
            var response = await _sender.Send(request);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return NotFound(Helpers.ErrorResponse(404,response.ErrorMessage));
        }
        [HttpGet("OnActor")]
        public async Task<IActionResult> RecommendedMoviesBasedOnActors([FromQuery] RecommendMoviesOnActorQuery request)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized();
            }
            request.UserId = userId;
            var response = await _sender.Send(request);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return NotFound(Helpers.ErrorResponse(404, response.ErrorMessage));
        }
    }
}
