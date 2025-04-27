
namespace MovieAPI.Repository
{
    public class ActorAwardRepo : BaseRepository<ActorAward>
    {
        public ActorAwardRepo(AppDBContext context) : base(context)
        {
        }

        public async Task<RepoResult<string>> CheckYearOfHonorOfAward(int awardId,int year)
        {
            if (await base.AnyAsync(x => x.AwardId == awardId && x.YearOfHonor == year))
                return RepoResult<string>
                    .ValidationError("The Award at that year is taken by an actor");
            return RepoResult<string>.SuccessValidation();
        }

        public async Task<RepoResult<ActorAward>> GetActorsAwards(int pageIndex,
        int pageSize,
        string orderType,
        int? actorId=null,
        int? yearOfHonor=null)
        {
            var query = _context.ActorAwards.AsQueryable();
            if (actorId.HasValue)
                query = query.Where(x => x.ActorId == actorId.Value);
            if (yearOfHonor.HasValue)
                query = query.Where(x=>x.YearOfHonor==yearOfHonor.Value);

            if (orderType == Helpers.OrderDescending)
                query = query.OrderByDescending(x => x.YearOfHonor);
            else query = query.OrderBy(x => x.YearOfHonor);


            var count = await query.CountAsync();
            query = query.Skip((pageIndex-1)*pageSize).Take(pageSize);
            
            query = query.Include(x => x.Award);
            query = query.Include(x => x.Actor);

            return RepoResult<ActorAward>
                .Result(await query.ToListAsync(), count);
        }

    }
}
