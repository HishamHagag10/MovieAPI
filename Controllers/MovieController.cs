
namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ISender _sender;

        public MovieController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMovieAsync([FromQuery] GetMoviesQuery request)
        {
            var response = await _sender.Send(request);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return NotFound(Helpers.ErrorResponse(response.StatusCode, response.ErrorMessage));
        }

        [HttpGet(template: "Get/{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMovieById([FromRoute] GetMovieByIdQuery request)
        {
            var response = await _sender.Send(request);
            if(response.IsSuccess)
                return Ok(response.Data);
            return NotFound(Helpers.ErrorResponse(response.StatusCode, response.ErrorMessage));
        }


        [HttpPost(template: "Add")]
        public async Task<IActionResult> AddMovieAsync([FromForm] AddMovieCommand request)
        {
            var response = await _sender.Send(request);
            if(response.IsSuccess)
                return Ok(response.Data);
            return BadRequest(Helpers.ErrorResponse(response.StatusCode, response.ErrorMessage));
        }

        [HttpPut(template: "Update/{id}")]
        public async Task<IActionResult> UpdateMovieAsync(int id, [FromForm] UpdateMovieCommand request)
        {
            request.Id = id;
            var response =await _sender.Send(request);
            if (response.IsSuccess)
                return Ok(response.Data);
            return StatusCode(response.StatusCode,Helpers.ErrorResponse(response.StatusCode, response.ErrorMessage));
        }

        [HttpDelete(template: "Delete/{Id}")]
        public async Task<IActionResult> DeleteMovieAsync([FromRoute] DeleteMovieCommand request)
        {
            var response =await _sender.Send(request);
            if (response.IsSuccess)
                return Ok(response.Data);
            return StatusCode(response.StatusCode, Helpers.ErrorResponse(response.StatusCode, response.ErrorMessage));
        }

        [HttpPost]
        [Route("{id}/Actors")]
        public async Task<IActionResult> AddActorsToMovie(int id, AddActorsToMovieCommand request)
        {
            request.MovieId = id;
            var response = await _sender.Send(request);
            if (response.IsSuccess)
                return Ok(response.Data);
            return StatusCode(response.StatusCode, Helpers.ErrorResponse(response.StatusCode, response.ErrorMessage));
        }

        [HttpGet]
        [Route("Actors")]
        public async Task<IActionResult> GetActorsOfMovie([FromQuery] GetMoviesActorsQuery request)
        {
            var response = await _sender.Send(request);
            if (response.IsSuccess)
                return Ok(response.Data);
            return StatusCode(response.StatusCode, Helpers.ErrorResponse(response.StatusCode, response.ErrorMessage));
        }

        [HttpPost("{MovieId}/Watch")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> MovieWatched([FromRoute] WatchMovieCommand request)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?
                .Value, out int userId))
            {
                return Unauthorized();
            }
            request.UserId = userId;
            var response = await _sender.Send(request);
            if (!response.IsSuccess)
                return NotFound(Helpers.ErrorResponse(404,response.ErrorMessage));
            return Ok();
        }
        [HttpGet("User")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> MovieWatchedByUser([FromQuery] GetUserMoviesQuery request)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?
                .Value, out int userId))
            {
                return Unauthorized();
            }
            request.UserId = userId;
            var response = await _sender.Send(request);
            if (!response.IsSuccess)
                return NotFound(Helpers.ErrorResponse(404, response.ErrorMessage));
            return Ok(response.Data);
        }

    }
}
