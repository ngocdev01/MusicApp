using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Domain.Common.Entities;
using MusicApp.Domain.Common.Errors;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MusicApp.Application.Services.Service;

public abstract class BaseService
{
    public async Task<T> GetEntityAsync<T>(IRepository<T> repository,string id) where T : EntityBase
    {
       
        if (await repository.GetAsync(id) is not T entity)
        {
            throw new HttpResponseException(System.Net.HttpStatusCode.NotFound,$"{typeof(T)} is not exists");
        }
        return entity;
    }
    public async Task<T> GetEntityAsync<T>(IRepository<T> repository, Expression<Func<T, bool>> func) where T : EntityBase
    {
        var list = await repository.WhereAsync(func);
        var entity = list.FirstOrDefault();
        return entity is not null ? entity : 
            throw new HttpResponseException(System.Net.HttpStatusCode.NotFound, $"{typeof(T)} is not exists");
    }

   
}
