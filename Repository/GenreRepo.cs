
namespace MovieAPI.Repository
{
    public class GenreRepo : BaseRepository<Genre>
    {
        public GenreRepo(AppDBContext context) : base(context)
        {
        }
        public async Task<RepoResult<Genre>> GetGenresAsync(int pageIndex,
        int pageSize,
        string orderBy,
        string orderType)
        {
            var query = base._context.Genres.AsQueryable();

            if (orderType == Helpers.OrderDescending)
                query = query.OrderByDescending(OrderExpression(orderBy));
            else query = query.OrderBy(OrderExpression(orderBy));

            var count = await query.CountAsync();

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return RepoResult<Genre>.Result(await query.ToListAsync(), count);
        }

        private Expression<Func<Genre, object?>> OrderExpression(string orderBy)
        {
            Expression<Func<Genre, object?>> ex = orderBy switch
            {
                nameof(Genre.Movies) => (a => a.Movies.Count()),
                _ => (a => a.Name)
            };
            return ex;
        }
    }
}
