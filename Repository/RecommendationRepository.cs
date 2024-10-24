using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Dtos;
using MovieAPI.Helper;
using MovieAPI.models;
using NetTopologySuite.Index.HPRtree;
using NetTopologySuite.Noding;

namespace MovieAPI.Repository
{
    public class RecommendationRepository 
    {
        private readonly AppDBContext _context;
        public RecommendationRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<PagenatedResponse<Movie>> RecommendationsMoviesbasedOnGenreAsync(int userId,int PageIndex=1,int PageSize=10)
        {
            var genres = await _context.UserMovies
                .Where(x => x.UserId == userId)
                .GroupBy(x=>x.Movie.GenreId)
                .OrderByDescending(x=>x.Count())
                .Select(x=>x.Key)
                .Take(3).ToListAsync();

            var query = _context.Movies
                .Where(x => genres.Contains(x.GenreId))
                .Where(x => !_context.UserMovies.Any(y => y.UserId == userId && y.MovieId == x.Id));
            
            var movies = await query.Include(x=>x.Genre).Skip(PageSize * (PageIndex - 1)).Take(PageSize)
                .ToListAsync();

            var response = new PagenatedResponse<Movie>
            {
                Data = movies,
                PageIndex = PageIndex,
                PageSize = PageSize,
                TotalPages = (query.Count()+PageSize-1)/PageSize
            };
            return response;
        }
        public async Task<PagenatedResponse<Movie>> RecommendationsMoviesbasedOnActorAsync(int userId, int PageIndex = 1, int PageSize = 10)
        {
            var actors = await _context.UserMovies
                .Where(x => x.UserId == userId)
                .SelectMany(x=>x.Movie.Actors)
                .GroupBy(x => x.Id).
                OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .Take(10)
                .ToListAsync();

            var query = _context.MoviesActors
                .Where(x => actors.Contains(x.ActorId))
                .Where(x => !_context.UserMovies.Any(y => y.UserId == userId 
                && y.MovieId == x.MovieId));
            
            var movies = await query.Include(x=>x.Movie).ThenInclude(x=>x.Genre).Select(x=>x.Movie)
                .Skip(PageSize * (PageIndex - 1))
                .Take(PageSize).ToListAsync();
            
            var response = new PagenatedResponse<Movie>
            {
                Data = movies,
                PageIndex = PageIndex,
                PageSize = PageSize,
                TotalPages = (query.Count() + PageSize - 1) / PageSize
            };
            return response;
        }

    }
}
