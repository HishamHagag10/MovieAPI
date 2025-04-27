
namespace MovieAPI.Repository
{
    public class MovieActorsRepo : BaseRepository<MoviesActors>
    {
        public MovieActorsRepo(AppDBContext context) : base(context)
        {
        }

        public async Task<RepoResult<MoviesActors>>  GetActorsMoviesAsync(
            int pageIndex,int pageSize,
            int? actorId, int? movieId)
        {
            var query = _context.MoviesActors.AsQueryable();
            if(actorId.HasValue)
                query=query.Where(x=>x.ActorId== actorId);
            if(movieId.HasValue)
                query=query.Where(x=>x.MovieId== movieId);
            
            query = query.OrderByDescending(x=>x.Salary);

            query=query.Skip((pageIndex-1)*pageSize).Take(pageSize);

            var count = await query.CountAsync();
            return RepoResult<MoviesActors>
                .Result(await query.ToListAsync(),count);
        }
    }
}
