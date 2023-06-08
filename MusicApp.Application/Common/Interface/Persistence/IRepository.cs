using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Common.Interface.Persistence;

public interface IRepository<T>  where T : EntityBase
{

    IQueryable<T> GetQuery();
    Task<IEnumerable<TValue>> GetListAsync<TValue>(IQueryable<TValue> values);
    Task<T?> GetAsync(string id);
    Task<T?> GetAsync(Expression<Func<T, bool>> func);
    Task<IEnumerable<T>> GetAll();
    Task AddAsync(T entity);
    Task AddAsync(ICollection<T> collection);
    Task RemoveAsync(string id);
   
    Task RemoveAsync(T entity);
    Task UpdateAsync(T entity, Action<T> action);

    Task SaveChangeAsync();
  
    Task GetRelatedAsync<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> prop) where TProperty : class;
    Task GetAllRelatedAsync(T entity);
    IAsyncEnumerable<T> WhereAsyncEnumerable(Expression<Func<T, bool>> func);
    Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> func);
    Task<IEnumerable<T>> WhereSkipTakeAsync(Expression<Func<T, bool>> func,int skil,int take);
    IQueryable<T> GetQueryInclude(params Expression<Func<T, object>>[] includes);
}
