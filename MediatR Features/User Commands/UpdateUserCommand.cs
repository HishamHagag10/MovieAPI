using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MovieAPI.MediatR_Features.User_Commands
{
    public class UpdateUserCommand:IRequest<RequestResult<bool>>
    {
        [BindNever]
        public int Id { get; set; }
        public string? Name { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
    }
}
