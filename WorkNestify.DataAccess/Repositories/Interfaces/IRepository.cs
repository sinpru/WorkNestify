using System.Linq.Expressions;

namespace WorkNestify.DataAccess.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetAsync(
        Expression<Func<T, bool>> filter, 
        string? includeProperties = null,
        bool? tracked = true);
    
    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null,
        Expression<Func<T, object>>[]? orderBy = null,
        Expression<Func<T, object>>[]? orderByDescending = null,
        int? skip = null,
        int? take = null);

    IQueryable<T> GetAllQueryable(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null,
        Expression<Func<T, object>>[]? orderBy = null,
        Expression<Func<T, object>>[]? orderByDescending = null,
        int? skip = null,
        int? take = null);
    
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
}