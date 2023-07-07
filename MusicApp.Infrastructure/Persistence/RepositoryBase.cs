using Microsoft.EntityFrameworkCore;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Domain.Common.Entities;
using MusicApp.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Infrastructure.Persistence;

public  class RepositoryBase<T> : IRepository<T> where T : EntityBase
{
    protected readonly MusicContext _musicContext;
    protected readonly DbSet<T> _dbSet;

    public RepositoryBase(MusicContext musicContext)
    {
        _musicContext = musicContext;
        _dbSet = _musicContext.Set<T>();
    }
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _musicContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(string id)
    {
        _dbSet.RemoveRange(_dbSet.Where(entity => entity.Id.Equals(id)));
        await _musicContext.SaveChangesAsync();
    }

    public  async Task<IEnumerable<T>> GetAll()
    {    
        return  await _dbSet.ToListAsync();
    }

    public IQueryable<T> GetQueryInclude(params Expression<Func<T, object>>[] includes)
    {
        var query = _dbSet.AsQueryable();
        foreach(var i in includes)
        {
            query =  query.Include(i);
        }
        return query;
    }

    public async Task<T?> GetAsync(string id)
    {
        return await _dbSet.FirstOrDefaultAsync(entity => entity.Id == id);
    }
    public async Task GetRelatedAsync<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> prop)
        where TProperty : class
    {
        var entry = _dbSet.Entry(entity);

        await entry.Collection(prop).LoadAsync();

    }

    public async Task GetAllRelatedAsync(T entity)
    {
        var entry = _dbSet.Entry(entity);
        foreach (var collection in entry.Collections)
        {
            await collection.LoadAsync();
        }
    }

    public IAsyncEnumerable<T> WhereAsyncEnumerable(Expression<Func<T, bool>> func)
    {
        return _dbSet.Where(func).AsQueryable().AsAsyncEnumerable();
    }
    public async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> func)
    {
        return await _dbSet.Where(func).ToListAsync();
    }
    public async Task AddAsync(ICollection<T> collection)
    {
        await _dbSet.AddRangeAsync(collection);
        await _musicContext.SaveChangesAsync();
    }
    


    public async Task RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _musicContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity, Action<T> action)
    {
        action(entity);
        await _musicContext.SaveChangesAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> func)
    {
        return await _dbSet.FirstOrDefaultAsync(func);
    }

    public IQueryable<T> GetQuery()
    {
        return _dbSet;
    }

    public async Task<IEnumerable<TValue>> GetListAsync<TValue>(IQueryable<TValue> values) 
    {
        return await values.ToListAsync();
    }

    public async Task<IEnumerable<T>> WhereSkipTakeAsync(Expression<Func<T, bool>> func, int skip, int take)
    {
        return await _dbSet.Where(func).Skip(skip).Take(take).ToListAsync();
    }

    public async Task SaveChangeAsync()
    {
        await _musicContext.SaveChangesAsync();
    }
}
