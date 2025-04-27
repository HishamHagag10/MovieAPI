

namespace MovieAPI.Repository
{
    public class RecommendationRepo 
    {
        private readonly AppDBContext _context;
        public RecommendationRepo(AppDBContext context)
        {
            _context = context;
        }
        public async Task<RepoResult<Movie>> RecommendationsMoviesbasedOnGenreAsync(int PageIndex, int PageSize,int userId)
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

            var movies = await query.Include(x => x.Genre)
                .Skip(PageSize * (PageIndex - 1)).Take(PageSize)
                .ToListAsync();
            return RepoResult<Movie>.Result(movies, await query.CountAsync());
        }
        public async Task<RepoResult<Movie>> RecommendationsMoviesbasedOnActorAsync(int PageIndex, int PageSize, int userId)
        {
            var actors = await _context.UserMovies
                .Where(x => x.UserId == userId)
                .SelectMany(x=>x.Movie.Actors)
                .GroupBy(x => x.Id)
                .OrderByDescending(x => x.Count())
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
            
            
            return RepoResult<Movie>.Result(movies, await query.CountAsync());
        }
    }
}
