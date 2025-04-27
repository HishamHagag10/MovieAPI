
namespace MovieAPI.Repository
{
    public class UserMovieRepo : BaseRepository<UserMovies>
    {
        public UserMovieRepo(AppDBContext context) : base(context)
        {
        }

        public async Task<RepoResult<UserMovies>> GetUserMovies(int pageIndex, int pageSize, 
            int userId)
        {
            var query = _context.UserMovies.AsQueryable();
            
            query=query.Where(x => x.UserId == userId);
            
            query=query.OrderByDescending(x => x.WatchedAt);
            var count = await query.CountAsync();  
            
            query=query.Skip((pageIndex-1)*pageSize).Take(pageSize);

            return RepoResult<UserMovies>.Result(await query.ToListAsync(),count);
        }
    }
}
