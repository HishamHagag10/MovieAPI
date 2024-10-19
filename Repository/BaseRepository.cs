using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieAPI.Helper;
using System.Linq.Expressions;
using System.Security.Cryptography.Xml;
using EFCore.BulkExtensions;

namespace MovieAPI.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDBContext _context;

        public BaseRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<PagenatedResponse<T>> GetAllAsync(int pageIndex = 1, int pagesize = 10)
        {
            return new PagenatedResponse<T>
            {
                Data = await _context.Set<T>().Skip(pagesize*(pageIndex-1)).Take(pagesize).ToListAsync(),
                PageIndex = pageIndex,
                PageSize = pagesize,
                TotalPages = ((await _context.Set<T>().CountAsync()) + pagesize-1)/pagesize,
            };
        }

        public async Task<T> AddAsync(T item)
        {
            await _context.AddAsync(item);
            //_context.SaveChanges();
            return item;
        }

        public async Task AddRangeAsync(IEnumerable<T> items)
        {
            await _context.BulkInsertAsync(items);
            //await _context.AddRangeAsync(items);
            return ;
        }

        public T Delete(T item)
        {
            _context.Remove(item);
            //_context.SaveChanges();
            return item;
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> criteria, string[]? includes = null)
        {
            var items = _context.Set<T>().Where(criteria);
            if (includes != null)
                foreach (var include in includes)
                    items = items.Include(include);
            return await items.ToListAsync();
        }
        public async Task<IEnumerable<T2>> FindAsync<T2>(Expression<Func<T, bool>> criteria, Expression<Func<T, T2>> selector, string[]? includes = null)
        {
            var items = _context.Set<T>().Where(criteria);
            if (includes != null)
                foreach (var include in includes)
                    items = items.Include(include);
            return await items.Select(selector).ToListAsync();
        }

        public async Task<PagenatedResponse<T>> FindAsync(Expression<Func<T, bool>> criteria, int pageIndex = 1, int pagesize = 10, string[]? includes = null)
        {
            var items = _context.Set<T>().Where(criteria);
            var count = ((await items.CountAsync()) + pagesize - 1) / pagesize;
            items=items.Skip(pagesize * (pageIndex - 1)).Take(pagesize);
            if (includes != null)
                foreach (var include in includes)
                    items = items.Include(include);
            return new PagenatedResponse<T>
            {
                Data = await items.ToListAsync(),
                PageIndex= pageIndex,
                PageSize=pagesize,
                TotalPages = count,
            };
        }

        public async Task<PagenatedResponse<T2>> FindAsync<T2>(Expression<Func<T, bool>> criteria, Expression<Func<T, T2>> selector, int pageIndex = 1, int pagesize = 10, string[]? includes = null)
        {
            var items = _context.Set<T>().Where(criteria);
            var count = ((await items.CountAsync()) + pagesize - 1) / pagesize;
            items=items.Skip(pagesize * (pageIndex - 1)).Take(pagesize);
            if (includes != null)
                foreach (var include in includes)
                    items = items.Include(include);
            await items.Select(selector).ToListAsync();
            return new PagenatedResponse<T2>
            {
                Data = await items.Select(selector).ToListAsync(),
                PageIndex= pageIndex,
                PageSize= pagesize,
                TotalPages = count,
            };
        }
        public async Task<T?> GetByIdAsync(int? id)
        {
            if (id == null) return null;
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<T?> FirstAsync(Expression<Func<T, bool>> e, string[]? includes=null)
        {
            /*var item = await _context.Set<T>().SingleOrDefaultAsync(e);
            if (item == null) return item;
            foreach (var inc in includes)
            {
                _context.Entry(item).Reference(inc).Load();
            }
            return item;*/
            IQueryable<T> items =  _context.Set<T>();
            if (includes != null)
            {
                foreach (var inc in includes)
                    items = items.Include(inc);
            }
            return await items.FirstOrDefaultAsync(e);
        }
        public async Task<T2?> FirstAsync<T2>(Expression<Func<T2, bool>> e, Expression<Func<T, T2>> s,string[]? includes = null)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includes != null)
            {
                foreach (var inc in includes)
                    items = items.Include(inc);
            }
            return await items.Select(s).FirstOrDefaultAsync(e);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> e)
        {
            return await _context.Set<T>().AnyAsync(e);
        }

        public T Update(T item)
        {
            _context.Update(item);
            //_context.SaveChanges();
            return item;
        }
        public async Task UpdateOrAdd(T item)
        {
            var keys = _context.Model.FindEntityType(item.GetType())?.FindPrimaryKey()?
                    .Properties;
           
            if (keys == null) return;
            IQueryable<T> query=_context.Set<T>();
            foreach (var key in keys)
            {
                var value = key.PropertyInfo?.GetValue(item);
                if(value == null) continue;
                query = query.Where(e => EF.Property<object>(e, key.Name).Equals(value));
            }
            T? existItem = null;
            
            existItem=await query.FirstOrDefaultAsync();

            if (existItem is null)
                _context.Add(item);
            else 
                _context.Entry(existItem).CurrentValues.SetValues(item);
        }
        public async Task UpdateOrAddRangeAsync(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                await UpdateOrAdd(item);   
            }
            return ;
        }

    }
}
