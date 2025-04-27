using System.ComponentModel.DataAnnotations;
using MediatR;
using MovieAPI.Models.Dtos;

namespace MovieAPI.MediatR_Features.User_Commands
{
    public class AuthenticateUserCommand:IRequest<RequestResult<AuthResult>>
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
