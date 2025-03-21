using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Interfaces;

namespace WorkNestify.DataAccess.Repositories.Implementations;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T?> GetAsync(
        Expression<Func<T, bool>> filter, 
        string? includeProperties = null, 
        bool? tracked = true)
    {
        IQueryable<T> query = _dbSet;
        if (tracked == true)
        {
            query = query.AsNoTracking();
        }
        query = query.Where(filter);
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var property in includeProperties
                         .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null,
        Expression<Func<T, object>>[]? orderBy = null,
        Expression<Func<T, object>>[]? orderByDescending = null,
        int? skip = null,
        int? take = null)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
            query = query.Where(filter);

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }

        if (orderBy != null && orderBy.Length > 0)
        {
            IOrderedQueryable<T> orderedQuery = query.OrderBy(orderBy[0]);
            for (int i = 1; i < orderBy.Length; i++)
            {
                orderedQuery = orderedQuery.ThenBy(orderBy[i]);
            }

            query = orderedQuery;
        }

        if (orderByDescending != null && orderByDescending.Length > 0)
        {
            IOrderedQueryable<T> orderedQuery = query.OrderByDescending(orderByDescending[0]);
            for (int i = 1; i < orderByDescending.Length; i++)
            {
                orderedQuery = orderedQuery.ThenByDescending(orderByDescending[i]);
            }

            query = orderedQuery;
        }

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync();
    }

    public IQueryable<T> GetAllQueryable(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null,
        Expression<Func<T, object>>[]? orderBy = null,
        Expression<Func<T, object>>[]? orderByDescending = null,
        int? skip = null,
        int? take = null)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
            query = query.Where(filter);

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }

        if (orderBy != null && orderBy.Length > 0)
        {
            IOrderedQueryable<T> orderedQuery = query.OrderBy(orderBy[0]);
            for (int i = 1; i < orderBy.Length; i++)
            {
                orderedQuery = orderedQuery.ThenBy(orderBy[i]);
            }

            query = orderedQuery;
        }

        if (orderByDescending != null && orderByDescending.Length > 0)
        {
            IOrderedQueryable<T> orderedQuery = query.OrderByDescending(orderByDescending[0]);
            for (int i = 1; i < orderByDescending.Length; i++)
            {
                orderedQuery = orderedQuery.ThenByDescending(orderByDescending[i]);
            }

            query = orderedQuery;
        }

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return query;
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? filter)
    {
        return filter == null
            ? await _dbSet.CountAsync()
            : await _dbSet.CountAsync(filter);
    }
}