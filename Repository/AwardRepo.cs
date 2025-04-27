
namespace MovieAPI.Repository
{
    public class AwardRepo : BaseRepository<Award>
    {
        public AwardRepo(AppDBContext context) : base(context)
        {
        }
        public async Task<int> CountAsync()
        {
            return await _context.Actors.CountAsync();
        }

        public async Task<RepoResult<Award>> GetAwardsAsync(int pageIndex,
        int pageSize,
        string orderType)
        {
            var query = base._context.Awards.AsQueryable();
            
            if (orderType == Helpers.OrderDescending)
                query = query.OrderByDescending(x=>x.Name);
            else query = query.OrderBy(x=>x.Name);

            var count = await query.CountAsync();
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return RepoResult<Award>.Result(await query.ToListAsync(), count);
        }

    }
}
