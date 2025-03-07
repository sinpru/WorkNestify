using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Locations;
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

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;
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

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;
        if (filter != null)
            query = query.Where(filter);
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var property in includeProperties
                         .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }
        return await query.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        if (entity is Province)
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Provinces ON");
        }
        else if (entity is District)
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Districts ON");
        }
        else if (entity is Ward)
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Wards ON");
        }
        
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        
        if (entity is Province)
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Provinces OFF");
        }
        else if (entity is District)
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Districts OFF");
        }
        else if (entity is Ward)
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Wards OFF");
        }
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}