using Microsoft.AspNetCore.Http;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Common.Interface.Services;

public interface IGenreService
{

    public Task<GenreInfo> Get(string id);
    public Task<GenreInfo> GetByName(string name);
    public Task<GenreInfo> Add(Genre song);

    public Task<IEnumerable<GenreInfo>> GetAll();
    public Task Delete(string id);
    public Task UpdateName(string id, string name);
    public Task<GenreInfo> Create(string name);
}
