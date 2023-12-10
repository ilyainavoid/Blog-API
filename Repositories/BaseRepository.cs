using System.Linq.Expressions;
using BlogApi.Repositories.Interfaces;
using BlogApi.Services;
using BlogApi.Services.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<T?> Get(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<List<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public IEnumerable<T> GetSpecified(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }

    public async Task Insert(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public async Task InsertCollection(List<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
    }

    public void DeleteCollection(List<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}