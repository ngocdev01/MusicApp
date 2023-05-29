using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Domain.Common.Entities;
using System.Xml.Linq;

namespace MusicApp.Application.Services.Service;

public class GenreService : BaseService, IGenreService
{
    private readonly IRepository<Genre> _genreRepository;
    

    public GenreService(IRepository<Genre> genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<GenreInfo> Add(Genre genre)
    {
        await _genreRepository.AddAsync(genre);
        return new GenreInfo(genre);
    }

    public async Task<GenreInfo> Create(string name)
    {
        Genre genre = new Genre()
        {
            Id = Guid.NewGuid().ToString(),
            Name = name

        };
        return await Add(genre);       
    }

    public async Task Delete(string id)
    {
        await _genreRepository.RemoveAsync(id);
    }

  

    public async Task<GenreInfo> Get(string id)
    {
        var gerne = await GetEntityAsync(_genreRepository, id);
        return new GenreInfo(gerne);
    }

    public async Task<IEnumerable<GenreInfo>> GetAll()
    {
        var genreList = await _genreRepository.GetAll();
        return genreList.Select(genre => new GenreInfo(genre));
    }

    public async Task<GenreInfo> GetByName(string name)
    {
        var genre = await GetEntityAsync(_genreRepository, genre => genre.Name == name);
        return new GenreInfo(genre);
    }

    public async Task UpdateName(string id, string name)
    {
        var genre = await GetEntityAsync(_genreRepository, id);
        await _genreRepository.UpdateAsync(genre, g => g.Name = name);
    }
}
