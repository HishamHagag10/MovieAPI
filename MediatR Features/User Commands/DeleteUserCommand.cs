using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MovieAPI.MediatR_Features.User_Commands
{
    public class DeleteUserCommand:IRequest<RequestResult<bool>>
    {
        [BindNever]
        public int Id { get; set; }
    }
}
