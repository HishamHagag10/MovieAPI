using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieAPI.models;
using MovieAPI.Models.Dtos;
using MovieAPI.Models.Dtos.Response_Dtos;

namespace MovieAPI.MediatR_Features.Movie_Commands
{
    public class AddActorsToMovieCommand : IRequest<RequestResult<bool>>
    {
        [BindNever]
        public int MovieId { get; set; }
        [Required]
        public IEnumerable<MovieActorDto> Actors { get; set; } 
            = Enumerable.Empty<MovieActorDto>();
        public IEnumerable<MoviesActors> ToModel()
        {
            var ret = new List<MoviesActors>();
            foreach (var actor in Actors)
            {
                ret.Add(new MoviesActors { 
                    MovieId=this.MovieId,
                    ActorId=actor.ActorId,
                    Salary=actor.Salary
                });
            }
            return ret;
        }
    }
}
