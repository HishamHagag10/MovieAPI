using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using MovieAPI.Authentication;
using MovieAPI.MediatR_Features.User_Commands;
using MovieAPI.models;
using MovieAPI.Models.Dtos;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.User_Commands_Handlers
{
    public class AuthenticateUserCommandhandler(UserRepo _userRepo,JwtOptions _jwtOptions)
        : IRequestHandler<AuthenticateUserCommand, RequestResult<AuthResult>>
    {
        public async Task<RequestResult<AuthResult>> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetUserByUserNameAndPasswordAsync(request.UserName!,request.Password!);
                
            if (user == null)
            {
                return RequestResult<AuthResult>.Failure(401,
                    "Incorrect UserName Or Password");
            }
            var authResult = GenerateToken(user);
            return RequestResult<AuthResult>.Success(authResult);
        }
        private AuthResult GenerateToken(User user)
        {
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
            return new AuthResult()
            {
                Result = true,
                Token = accessToken,
            };
        }
    }
}
