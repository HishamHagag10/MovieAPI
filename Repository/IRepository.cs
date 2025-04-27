
namespace MovieAPI.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<ICollection<T>> GetAllAsync(int pageIndex = 1, int pagesize = 10);
        Task<T?> GetByIdAsync(int? id);
        Task<int> SaveChangesAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> criteria);
        Task<T> AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> items);
        T Update(T item);
        T Delete(T item);
    }
}
