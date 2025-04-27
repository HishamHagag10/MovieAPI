
namespace MovieAPI.Repository
{
    public class MovieRepo : BaseRepository<Movie>
    {
        public MovieRepo(AppDBContext context) : base(context)
        {
        }

        public async Task<RepoResult<string>> ValidateMovieAsync(Movie movie)
        {
            if (await _context.Movies.AnyAsync(m => m.Link == movie.Link))
            {
                return RepoResult<string>.ValidationError("The link is exist, add Unique Link");
            }
            if (await _context.Movies.AnyAsync(m => m.Title == movie.Title))
            {
                return RepoResult<string>.ValidationError("The Title is exist, add Unique Link");
            }
            return RepoResult<string>.SuccessValidation();
        }

        public async Task<RepoResult<Movie>> GetMoviesAsync(int pageIndex,
            int pageSize,string orderBy, string orderType,
            int? actorId, int? genreId,int? userId)
        {
            var query = _context.Movies.AsQueryable();
            if (actorId.HasValue)
                query=query.Where(x => x.Actors.Any(a => a.Id == actorId));
  
            if (genreId.HasValue)
                query = query.Where(m => m.GenreId == genreId);
            
            if (userId.HasValue)
                query = query.Where(m => m.UsersWatched.Any(u => u.Id == userId));

            var count = query.Count();

            if (orderType == Helpers.OrderAscending)
                query = query.OrderBy(OrderExpression(orderType));
            else query = query.OrderByDescending(OrderExpression(orderType));

            query = query.Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize);

            query=query.Include(x => x.Genre);
            
            return RepoResult<Movie>.Result(await query.ToListAsync(),count);
        }

        private Expression<Func<Movie, object?>> OrderExpression(string orderBy)
        {
            Expression<Func<Movie, object?>> ex = orderBy switch
            {
                nameof(Movie.Title) => (a => a.Title),
                nameof(Movie.Rate) => (a => a.Rate),
                nameof(Movie.Actors) => (a => a.Actors.Count()),
                _ => (a => a.year)
            };
            return ex;
        }
    }
}
