using Microsoft.EntityFrameworkCore.Query;
using MovieAPI.Helper;
using MovieAPI.models;
using System.Linq.Expressions;

namespace MovieAPI.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<PagenatedResponse<T>> GetAllAsync(int pageIndex = 1, int pagesize = 10);
        Task<T?> GetByIdAsync(int? id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> criteria, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null);
        Task<PagenatedResponse<T>> FindPagenatedAsync(Expression<Func<T, bool>> criteria,int pageIndex=1,int pagesize = 10, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
        Task<IEnumerable<T2>> FindAsync<T2>(Expression<Func<T, bool>> criteria, Expression<Func<T, T2>> selector, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null);
        Task<PagenatedResponse<T2>> FindPagenatedAsync<T2>(Expression<Func<T, bool>> criteria, 
            Expression<Func<T, T2>> selector,
            int pageIndex = 1, int pagesize = 10, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBeforeBy = null,
            Func<IQueryable<T2>, IOrderedQueryable<T2>>? orderAfterBy = null
            );

        //        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> e, string[]? includes = null);
        //      Task<IEnumerable<T2>> FindAsync<T2>(Expression<Func<T, bool>> e, Expression<Func<T, T2>> s, string[]? includes = null);

        Task<T?> FirstAsync(Expression<Func<T, bool>> criteria, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null);
        Task<T2?> FirstAsync<T2>(Expression<Func<T2, bool>> criteria, Expression<Func<T, T2>> selector, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null);

        Task<bool> AnyAsync(Expression<Func<T, bool>> criteria);
        Task<T> AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> items);
        T Update(T item);
        Task UpdateOrAdd(T item);
        Task UpdateOrAddRangeAsync(IEnumerable<T> items);
        T Delete(T item);
        //T Include(T item, string[] includes);

    }
}
