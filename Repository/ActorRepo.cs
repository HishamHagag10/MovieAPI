
namespace MovieAPI.Repository
{
    public class ActorRepo : BaseRepository<Actor>
    {
        public ActorRepo(AppDBContext context) : base(context)
        {
        }

        public async Task<RepoResult<Actor>> CheckActorsIdsExist(IEnumerable<int>Ids)
        {
            var existIds= await _context.Actors
                .Where(x=>Ids.Contains(x.Id))
                .Select(x=>x.Id).ToListAsync();
            if (Ids.Except(existIds).Any())
            {
                return RepoResult<Actor>.ValidationError("Some Actor Id don't exist");
            }
            return RepoResult<Actor>.SuccessValidation();
        }
        public async Task<RepoResult<Actor>> GetActorsAsync(int pageIndex,
        int pageSize,
        string orderBy,
        string orderType,
        int? movieId = null,
        int? awardId = null)
        {
            var query = base._context.Actors.AsQueryable();
            if (movieId.HasValue)
            {
                query = query.Where(x=>x.Movies.Any(m=>m.Id == movieId.Value));
            }
            if (awardId.HasValue)
            {
                query = query.Where(x=>x.Awards.Any(a=>a.Id== awardId.Value));
            }
            if (orderType == Helpers.OrderDescending)
                query=query.OrderByDescending(OrderExpression(orderBy));
            else query=query.OrderBy(OrderExpression(orderBy));
            
            var count = await query.CountAsync();
            query=query.Skip((pageIndex-1)*pageSize).Take(pageSize);

            return RepoResult<Actor>.Result(await query.ToListAsync(), count);
        }

        public async Task<RepoResult<string>> ValidateActorAsync(Actor actor)
        {
            var emailExist =await _context.Actors.AnyAsync(x=>x.Email== actor.Email);
            if(emailExist)
                return RepoResult<string>.ValidationError("The Email is exist, use unique Email");
            return RepoResult<string>.SuccessValidation();
        }

        private Expression<Func<Actor,object?>> OrderExpression(string orderBy)
        {
            Expression<Func<Actor, object?>> ex = orderBy switch
            {
                nameof(Actor.Email) => (a=>a.Email),
                nameof(Actor.Movies) => (a => a.Movies.Count()),
                nameof(Actor.Awards) => (a =>a.Awards.Count()),
                _=>(a=>a.Name)
            };
            return ex;
        } 
    }
}
