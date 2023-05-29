using Microsoft.AspNetCore.Http;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Domain.Common.Entities;
namespace MusicApp.Application.Services.Service;

public class AlbumService : BaseService, IAlbumService
{
    private readonly IRepository<Album> _albumRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IRepository<Artist> _artistRepository;
    private readonly IRepository<User> _userRepository;
    public AlbumService(IRepository<Album> albumRepository, 
                        IFileRepository fileRepository,
                        IRepository<Artist> artistRepository,
                        IRepository<User> userRepository)
    {
        _albumRepository = albumRepository;
        _fileRepository = fileRepository;
        _artistRepository = artistRepository;
        _userRepository = userRepository;
    }
    public async Task AddAlbum(Album album)
    {
        await _albumRepository.AddAsync(album);
    }

    public async Task<AlbumResult> GetAlbum(string id)
    {
        var album = await GetEntityAsync(_albumRepository,id);
        return new AlbumResult(album);
       
    }

 

    public async Task<IEnumerable<AlbumResult>> GetAll()
    {
        var list = await _albumRepository.GetAll();
        return list.Select(album => new AlbumResult(album));    
    }

    public Task<IEnumerable<AlbumResult>>  GetSongsByAlbum(string albumId)
    {
        throw new NotImplementedException();
    }

    public async Task<AlbumResult> CreateAlbum(string Name, string[]? artists, IFormFile? image)
    {
        Album album = new();
        album.Id = Guid.NewGuid().ToString();
        album.Name = Name;
        if(artists!=null)
        {
            List<Artist> artistList = new();
            foreach (var artistId in artists)
            {
                artistList.Add(await GetEntityAsync(_artistRepository, artistId));
            }
            album.Artists = artistList;
        }

        if (image is not null)
            album.Image = await _fileRepository.UploadImageAsync(image);
        await _albumRepository.AddAsync(album);
        return new AlbumResult(album);
    }

    public async Task<AlbumResult> GetAlbumByName(string name)
    {
        var album = await GetEntityAsync(_albumRepository, a => a.Name == name);
        return new AlbumResult(album);
    }

    public async Task<IEnumerable<AlbumInfo>> GetTopAlbum(int top,string? orderBy)
    {
        var query = _albumRepository.GetQuery();
        if (orderBy == "songCount")
            query = query.OrderByDescending(album => album.Songs.Count);
        query = query.Take(top);
        var list = (await _albumRepository.GetListAsync(query));
        List<AlbumInfo> albums = new();
        foreach(var album in list)
        {
            albums.Add(new AlbumInfo(album));
        }
        return albums;
    }

    public async Task AlbumPlayEvent(string id,string userId)
    {
        var user =  await GetEntityAsync(_userRepository, userId);
        var album = await  GetEntityAsync(_albumRepository, id);

        await _albumRepository.UpdateAsync(album, a => a.UserAlbumEvents.Add(new UserAlbumEvent()
        {
            AlbumId = album.Id,
            UserId = user.Id,
            Time = DateTime.Now,

        }));
    }

    public async Task<IEnumerable<AlbumInfo>> GetAlbumByPlayTime(DateTime? from,DateTime? to,int? skip,int? take)
    {
        var query = _albumRepository.GetQuery();
        query = query.OrderByDescending(a => 
            a.UserAlbumEvents.Where( e =>
                (from != null ? e.Time > from : true) 
                && 
                (to != null? e.Time < to : true))
            .Count());
        if (skip != null) query = query.Skip((int)skip);
        if (take != null) query = query.Take((int)take);
        var albums =  await _albumRepository.GetListAsync(query);

        return albums.Select(a => new AlbumInfo(a));  
    }
}
