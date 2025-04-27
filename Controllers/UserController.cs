namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;

        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> AuthenticateUser(AuthenticateUserCommand request)
        {
            var response = await _sender.Send(request);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return Unauthorized(Helpers.ErrorResponse(401, response.ErrorMessage));
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterUserCommand request)
        {
            var response = await _sender.Send(request);
            if (response.IsSuccess)
            {
                return await AuthenticateUser(new AuthenticateUserCommand
                {
                    UserName = request.UserName,
                    Password = request.Password
                });
            }
            return Unauthorized(Helpers.ErrorResponse(401, response.ErrorMessage));
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteUser()
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id))
            {
                return Unauthorized();
            }
            DeleteUserCommand request = new DeleteUserCommand
            {
                Id = id
            };
            var response = await _sender.Send(request);
            if (response.IsSuccess)
            {
                return Ok();
            }
            return Unauthorized(Helpers.ErrorResponse(401, response.ErrorMessage));
        }

        [HttpPut("Update")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand request)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id))
            {
                return Unauthorized();
            }
            request.Id = id;
            var response = await _sender.Send(request);
            if (response.IsSuccess)
            {
                return Ok();
            }
            return Unauthorized(Helpers.ErrorResponse(401, response.ErrorMessage));

        }

    }
}
