using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MovieAPI.MediatR_Features.Movie_Commands
{
    public class WatchMovieCommand :IRequest<RequestResult<bool>>
    {
        [Required]
        [FromRoute]
        public int MovieId {  get; set; }
        [BindNever]
        public int UserId { get; set; }
    }
}
