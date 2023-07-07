using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Domain.Common.Entities;
using MusicApp.Domain.Common.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Services.Service;

public class UserService : BaseService, IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserSongEvent> _songEventRepository;
    private readonly IRepository<UserAlbumEvent> _albumEventRepository;
    private readonly IRepository<Song> _songRepository;
    private readonly IRepository<Album> _albumRepository;
    private readonly IFileStorageAdapter _fileStorageAdapter;

    public UserService(IRepository<User> userRepository,
                       IRepository<UserSongEvent> songEventRepository,
                       IRepository<Song> songRepository,
                       IRepository<UserAlbumEvent> albumEventRepository,
                       IRepository<Album> albumRepository,
                       IFileStorageAdapter fileStorageAdapter)
    {
        _userRepository = userRepository;
        _songEventRepository = songEventRepository;
        _songRepository = songRepository;
        _albumEventRepository = albumEventRepository;
        _albumRepository = albumRepository;
        _fileStorageAdapter = fileStorageAdapter;
    }

    public async Task CreateUser(UserInfo userInfo)
    {
        User user = new User()
        {
            UserName = userInfo.UserName.Trim(),
            Email = userInfo.Email.Trim(),
            Password = userInfo.Password.Trim(),
            RoleId = userInfo.RoleId.Trim(),
        };
       await _userRepository.AddAsync(user);
    }

    public async Task Delete(string id)
    {
        var user = await GetEntityAsync(_userRepository, id);
        await _userRepository.RemoveAsync(id);       
    }

    public async Task<IEnumerable<UserInfo>> GetAll()
    {
        return (await _userRepository.GetAll())
            .Select(u => new UserInfo(u.Id,u.UserName,u.Email,u.Password,u.RoleId));
    }

    public async Task<IEnumerable<AlbumInfo>> GetBestAlbumInMonth(string id)
    {
        var now = DateTime.Now;
        var user = await GetEntityAsync(_userRepository, id);
        var query = _albumEventRepository.GetQuery()
            .Where(e => e.UserId == user.Id && e.Time.Month == now.Month && e.Time.Year == now.Year)
            .GroupBy(e => e.AlbumId)
            .Select(s => new { Album = s.Key, Count = s.Count() })
            .OrderByDescending(s => s.Count)
            .Select(s => s.Album);

        var albumList = await _albumEventRepository.GetListAsync(query);

        List<AlbumInfo> results = new List<AlbumInfo>();
        foreach (string s in albumList)
        {
            var album = await GetEntityAsync(_albumRepository, s);
            results.Add(new AlbumInfo(album,_fileStorageAdapter));
        }

        return results;

    }

    public async Task<IEnumerable<SongResult>> GetBestSongInMonth(string id,int? skip,int? take)
    {
        var now = DateTime.Now;
        var user =await GetEntityAsync(_userRepository, id);

        var query =  _songEventRepository.GetQuery()
            .Where(e => e.UserId == user.Id && e.Time.Month == now.Month && e.Time.Year == now.Year)
            .GroupBy(e => e.SongId)
            .Select( s => new {Song = s.Key,Count = s.Count()})
            .OrderByDescending(s => s.Count)
            .Select(s => s.Song);
        if (skip.HasValue)
            query = query.Skip(skip.Value);
        if (take.HasValue)
            query = query.Take(take.Value);

        var songList =await _songEventRepository.GetListAsync(query);

        List < SongResult > results = new List<SongResult>();
        foreach (string s in songList)
        {
            var song = await GetEntityAsync(_songRepository, s);
            results.Add(new SongResult(song, _fileStorageAdapter));
        }

        return results;

        
    }

    public async Task<UserInfo> GetUser(string id)
    {
        var user = await GetEntityAsync(_userRepository, id);
        return new UserInfo(user.Id,user.UserName,user.Email,user.Password,user.RoleId);
    }


    public async Task<UserInfo> Update(string id, UserInfo user)
    {
        var u = await GetEntityAsync(_userRepository, id);
        if (u.Id != user.Id) 
            throw new HttpResponseException(System.Net.HttpStatusCode.NotFound, "User not exist");
        await _userRepository.UpdateAsync(u, u =>
        {
            u.UserName = user.UserName;
            u.Email = user.Email;
            u.RoleId = user.RoleId;

        });
        return new UserInfo(u.Id, u.UserName, u.Email, u.Password, u.RoleId);
       
    }   
}
