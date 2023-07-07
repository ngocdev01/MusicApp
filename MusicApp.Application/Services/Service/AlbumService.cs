using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Domain.Common.Entities;
using MusicApp.Domain.Common.Errors;

namespace MusicApp.Application.Services.Service;

public class AlbumService : BaseService, IAlbumService
{
    private readonly IRepository<Album> _albumRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IRepository<Artist> _artistRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserAlbumEvent> _userAlbumEventRepository;
    private readonly IFileStorageAdapter _fileStorageAdapter;
    private readonly IRepository<Song> _songRepository;
    public AlbumService(IRepository<Album> albumRepository, 
                        IFileRepository fileRepository,
                        IRepository<Artist> artistRepository,
                        IRepository<User> userRepository,
                        IFileStorageAdapter fileStorageAdapter,
                        IRepository<UserAlbumEvent> userAlbumEventRepository,
                        IRepository<Song> songRepository)
    {
        _albumRepository = albumRepository;
        _fileRepository = fileRepository;
        _artistRepository = artistRepository;
        _userRepository = userRepository;
        _fileStorageAdapter = fileStorageAdapter;
        _userAlbumEventRepository = userAlbumEventRepository;
        _songRepository = songRepository;
    }
    public async Task AddAlbum(Album album)
    {
        await _albumRepository.AddAsync(album);
    }

    public async Task<AlbumResult> GetAlbum(string id)
    {
        var album = await GetEntityAsync(_albumRepository,id);

        return new AlbumResult(album, _fileStorageAdapter);

    }

 

    public async Task<IEnumerable<AlbumInfo>> GetAll()
    {
        var list = await _albumRepository.GetAll();
        
        return list.Select(album => new AlbumInfo(album,_fileStorageAdapter));    
    }

    public Task<IEnumerable<AlbumResult>>  GetSongsByAlbum(string albumId)
    {
        throw new NotImplementedException();
    }

    public async Task<AlbumResult> CreateAlbum(string name, string[]? artists, string? image)
    {
        try
        {
            var albumImage = image!=null ? await _fileRepository.GetFilePath(FileType.Image,image) : null;
            try
            {
                Album album = new();
                album.Id = Guid.NewGuid().ToString();
                album.Name = name;
                album.Image = image;
                if (artists != null)
                {
                    List<Artist> artistList = new();
                    foreach (var artistId in artists)
                    {
                        artistList.Add(await GetEntityAsync(_artistRepository, artistId));
                    }
                    album.Artists = artistList;
                }

                await _albumRepository.AddAsync(album);
                return new AlbumResult(album, _fileStorageAdapter);
            }
            catch (Exception e)
            {
                if(albumImage!=null)
                    await _fileRepository.DeleteAsync(albumImage);
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, e.Message);
            }
        }
        catch (Exception e)
        {
            throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<AlbumResult> GetAlbumByName(string name)
    {
        var album = await GetEntityAsync(_albumRepository, a => a.Name == name);
        return new AlbumResult(album,_fileStorageAdapter);
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
            albums.Add(new AlbumInfo(album,_fileStorageAdapter));
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

        return albums.Select(a => new AlbumInfo(a,_fileStorageAdapter));  
    }

    public async Task DeleteAlbum(string id)
    {
        var album = await GetEntityAsync(_albumRepository, id);
        if(album.Image!=null) await _fileRepository.DeleteAsync(await _fileRepository.GetFilePath(FileType.Image, album.Image));
        await _albumRepository.RemoveAsync(album);
    }


}
