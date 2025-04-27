

namespace MovieAPI.Repository
{
    public class ReviewRepo : BaseRepository<Review>
    {
        public ReviewRepo(AppDBContext context) : base(context)
        {
        }

        public async Task<Review?> GetReviewBelongToUser(int reviewId,int UserId)
        {
            return await _context.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId && x.UserId == UserId);
        }
        public async Task<RepoResult<Review>> GetReviewsAsync(int pageIndex, int pageSize, 
            string orderBy, string orderType, int? movieId, int? userId)
        {
            var query = _context.Reviews.AsQueryable();
            if (movieId.HasValue)
            {
                query=query.Where(x=>x.MovieId == movieId);
            }
            if (userId.HasValue)
            {
                query=query.Where(x=>x.UserId == userId);
            }
            int count=query.Count();
            if (orderType == Helpers.OrderAscending)
                query = query.OrderBy(OrderExpression(orderBy));
            else 
                query = query.OrderByDescending(OrderExpression(orderBy));

            query=query.Skip((pageIndex-1)*pageSize).Take(pageSize);
            
            return RepoResult<Review>.Result(await query.ToListAsync(),count);
        }

        private Expression<Func<Review, object?>> OrderExpression(string orderBy)
        {
            Expression<Func<Review, object?>> ex = orderBy switch
            {
                nameof(Review.Rate) => (a => a.Rate),
                nameof(Review.CreatedAt) => (a => a.CreatedAt),
                _ => (a => a.UpdatedAt)
            };
            return ex;
        }
    }
}
