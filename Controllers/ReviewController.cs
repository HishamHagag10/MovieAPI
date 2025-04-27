
namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class ReviewController : ControllerBase
    {
        private readonly ISender _sender;

        public ReviewController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewsAsync([FromQuery] GetReviewsQuery request)
        {
            var response = await _sender.Send(request);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return NotFound(Helpers.ErrorResponse(response.StatusCode, response.ErrorMessage));
        }

        [HttpGet("Get/{Id}")]
        public async Task<IActionResult> GetReviewById([FromRoute] GetReviewByIdQuery request)
        {
            var response = await _sender.Send(request);
            if (response.IsSuccess)
                return Ok(response.Data);
            return BadRequest(Helpers.ErrorResponse(response.StatusCode, response.ErrorMessage));
        }

        [HttpPost("Add/")]
        public async Task<IActionResult> AddReview(AddReviewCommand request)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized();
            }
            request.UserId=userId;
            
            var response = await _sender.Send(request);
            if (response.IsSuccess)
                return Ok(response.Data);
            return BadRequest(Helpers.ErrorResponse(response.StatusCode, response.ErrorMessage));

        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateReview(int id, UpdateReviewCommand request)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized();
            }
            request.UserId= userId;

            request.Id=id;
            var response = await _sender.Send(request);
            if (response.IsSuccess)
                return Ok(response.Data);
            return BadRequest(Helpers.ErrorResponse(response.StatusCode, response.ErrorMessage));
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> DeleteReview([FromRoute] DeleteReviewCommand request)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized();
            }
            request.UserId = userId;
            var response = await _sender.Send(request);
            if (response.IsSuccess)
                return Ok(response.Data);
            return BadRequest(Helpers.ErrorResponse(response.StatusCode,
                response.ErrorMessage));
        }

    }
}
