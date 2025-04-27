
namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class GenreController : ControllerBase
    {

        private readonly ISender _sender;

        public GenreController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetGenersAsync([FromQuery]GetGenresQuery request)
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
        public async Task<IActionResult> GetByIdAsync([FromRoute] GetGenreByIdQuery request)
        {
            var response = await _sender.Send(request);
            if (!response.IsSuccess)
            {
                return NotFound(Helpers.ErrorResponse(404,response.ErrorMessage));
            }
            return Ok(response.Data);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddGenre(AddGenreCommand request)
        {
            var response = await _sender.Send(request);
            if(!response.IsSuccess)
                return BadRequest(Helpers.ErrorResponse(404, response.ErrorMessage));

            return Ok(response.Data);
        }

        [HttpPut(template: "Update/{id}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] UpdateGenreCommand request)
        {
            request.Id= id;
            var response = await _sender.Send(request);
            if (!response.IsSuccess)
            {
                return StatusCode(response.StatusCode,Helpers.ErrorResponse(response.StatusCode,response.ErrorMessage));
            }
            return Ok(response.Data);
        }


        [HttpDelete(template: "Delete/{Id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] DeleteGenreCommand request)
        {
            var response = await _sender.Send(request);
            if(!response.IsSuccess)
                return NotFound(Helpers.
                    ErrorResponse(response.StatusCode,
                    response.ErrorMessage));
            return Ok(response.Data);
        }
    }
}
