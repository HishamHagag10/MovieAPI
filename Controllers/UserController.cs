using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieAPI.Authentication;
using MovieAPI.Dtos;
using MovieAPI.models;
using MovieAPI.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JwtOptions _jwtOptions;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper,JwtOptions jwtOptions)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtOptions = jwtOptions;
        }
        [AllowAnonymous]
        [HttpGet("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser(LoggingModel model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return Unauthorized("Enter UserName And Password");
            }
            var user = await _unitOfWork.Users.FirstAsync(x=>x.UserName==model.UserName 
            && x.Password==model.Password);
            if (user == null)
            {
                return Unauthorized("Wrong UserName Or Password");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.Name),
                    new(ClaimTypes.Role,user.Role.ToString())
                })
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return Ok(accessToken);
        }
        
        [HttpPost("RegisterUser/")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromForm] AddUserDto dto)
        {
            if (await _unitOfWork.Users.AnyAsync(x => x.UserName == dto.UserName))
                return BadRequest("Change UserName");
            var user = _mapper.Map<AddUserDto,User>(dto);
            user.Role = Role.User;
            user = await _unitOfWork.Users.AddAsync(user);
            _unitOfWork.SaveChanges();
            return await AuthenticateUser(new LoggingModel { UserName=user.UserName,Password=user.Password});
        }

        [HttpDelete("DeleteUser")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteUser()
        {
            if(!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value,out int id))
            {
                return Unauthorized();
            }
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return NotFound();
            _unitOfWork.Users.Delete(user);
            _unitOfWork.SaveChanges();
            return Ok("Deleted Successfully");
        }

        [HttpPut("UpdateUser")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto dto)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id))
            {
                return Unauthorized();
            }

            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return NotFound();
            
            if(dto.Name != null)user.Name = dto.Name;
            if(dto.Email != null)user.Email = dto.Email;
            
            _unitOfWork.Users.Update(user);
            _unitOfWork.SaveChanges();
            return Ok("Updated Successfully");
        }
    }
}
