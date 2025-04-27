using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MovieAPI.MediatR_Features.User_Commands
{
    public class RegisterUserCommand : IRequest<RequestResult<bool>>
    {
        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
