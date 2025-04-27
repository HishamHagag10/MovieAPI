using EFCore.BulkExtensions;

namespace MovieAPI.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly AppDBContext _context;

        public BaseRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<ICollection<T>> GetAllAsync(int pageIndex = 1, int pagesize = 10)
        {
            return await _context.Set<T>().AsNoTracking().Skip(pagesize * (pageIndex - 1)).Take(pagesize).ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int? id)
        {
            if (id == null) return null;
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<T> AddAsync(T item)
        {
            await _context.AddAsync(item);
            return item;
        }

        public async Task AddRangeAsync(IEnumerable<T> items)
        {
            await _context.BulkInsertAsync(items);
            //await _context.AddRangeAsync(items);
            return ;
        }
        public T Update(T item)
        {
            _context.Update(item);
            //_context.SaveChanges();
            return item;
        }
        public T Delete(T item)
        {
            _context.Remove(item);
            return item;
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> e)
        {
            return await _context.Set<T>().AnyAsync(e);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
