using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Domain.Common.Entities;
using System.Linq;
using System.Net;
using MusicApp.Application.Services.DTOs.Result;
using Microsoft.AspNetCore.Http;

using MusicApp.Application.Services.DTOs.ObjectInfo;

namespace MusicApp.Application.Services.Service;

public class SongService : BaseService, ISongService
{
    private readonly IRepository<Song> _songRepository;
    private readonly IRepository<Album> _albumRepository;
    private readonly IRepository<Genre> _genreRepository;
    private readonly IRepository<Artist> _artistRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserSongEvent> _userEventRepository;
    private readonly IFileRepository _fileRepository;
    public SongService(IRepository<Song> songRepository,
                       IRepository<Album> albumRepository,
                       IRepository<Genre> genreRepository,
                       IRepository<Artist> artistRepository,
                       IFileRepository fileRepository,
                       IRepository<User> userRepository,
                       IRepository<UserSongEvent> userEventRepository)
    {
        _songRepository = songRepository;
        _albumRepository = albumRepository;
        _genreRepository = genreRepository;
        _artistRepository = artistRepository;
        _fileRepository = fileRepository;
        _userRepository = userRepository;
        _userEventRepository = userEventRepository;
    }

    public async Task<SongResult> AddSong(Song song)
    {
        await _songRepository.AddAsync(song);
        return new SongResult(song);
    }

    public async Task DeleteSongById(string id)
    {
        await GetEntityAsync(_songRepository, id);
        await _songRepository.RemoveAsync(id);
    }

    public async Task<IEnumerable<SongResult>> GetAll()
    {
        var query = _songRepository.GetQueryInclude(s => s.Albums, s => s.Playlists, s => s.Genres, s => s.Artists);
        var list = await _songRepository.GetListAsync(query);
        return list.Select(song => new SongResult(song));
    }


    public async Task UpdateSongArtist(string id, string artistId)
    {
        var artist = await GetEntityAsync(_artistRepository, artistId);
        var song = await GetEntityAsync(_songRepository, id);
        await _songRepository.UpdateAsync(song, song =>
        {
            if (!song.Artists.Contains(artist))
            {
                song.Artists.Add(artist);
            }
        });
    }

    public async Task UpdateSongCount(string id, int count)
    {
        var song = await GetEntityAsync(_songRepository, id);
        await _songRepository.UpdateAsync(song, song => song.Count++);
    }



    public async Task UpdateSongName(string id, string name)
    {
        var song = await GetEntityAsync(_songRepository, id);
        await _songRepository.UpdateAsync(song, s => s.Name = name);
    }

    public async Task<SongResult> GetSongById(string id)
    {
        var song = await GetEntityAsync(_songRepository, id);
        return new SongResult(song);
    }


    public async Task UpdateSongAlbum(string id, string albumId)
    {
        var song = await GetEntityAsync(_songRepository, id);
        var album = await GetEntityAsync(_albumRepository, albumId);
        await _songRepository.UpdateAsync(song, song =>
        {
            if (!song.Albums.Contains(album))
            {
                song.Albums.Add(album);
            }
        });
    }

    public async Task<SongResult> CreateSong(string name,
                                       string album,
                                       string[] artists,
                                       string[]? genres,
                                       IFormFile audio)
    {

        var songAlbum = await GetEntityAsync(_albumRepository, album);
        List<Artist> artistList = new();
        foreach (var artist in artists)
        {
            artistList.Add(await GetEntityAsync(_artistRepository, artist));
        }


        var audioSource = await _fileRepository.UploadAudioAsync(audio);
        Song newSong = new Song()
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            Source = audioSource,
            Artists = artistList,
            Albums = new List<Album>() { songAlbum },
        };
        if (genres != null)
        {
            List<Genre> genreList = new();
            foreach (var genre in genres)
            {
                genreList.Add(await GetEntityAsync(_genreRepository, genre));
            }
            newSong.Genres = genreList;
        }

        await _songRepository.AddAsync(newSong);

        return new SongResult(newSong);

    }

    public async Task<SongResult> GetSongByName(string name)
    {
        var song = await GetEntityAsync(_songRepository, s => s.Name == name);
        return new SongResult(song);
    }

    public async Task SongPlayEvent(string id, string userId)
    {
        var user = await GetEntityAsync(_userRepository, userId);
        var song = await GetEntityAsync(_songRepository, id);
        await _songRepository.UpdateAsync(song, s => 
        { 
            s.UserSongEvents.Add(new UserSongEvent() 
            { 
                SongId = id, 
                UserId = userId, 
                Time = DateTime.Now 
            }); 
            s.Count += 1; 
        });

    }

    public async Task<IEnumerable<string>> GetUsers(string id)
    {
        var song = await GetEntityAsync(_songRepository, id);
        List<string> users = new List<string>();
        foreach (var e in song.UserSongEvents)
        {
            users.Add(e.UserNavigation.Id);
        }
        return users;
    }

    public async Task<IEnumerable<SongResult>> Recommend(params string[] id)
    {
        var r = new Recommend();
        var model = r.LoadModel(@"D:\Data\Model");
        var songs = r.RecommendSong(model, id);
        List<SongResult> result = new List<SongResult>();
        foreach (var i in songs)
            result.Add(new SongResult(await GetEntityAsync(_songRepository, i)));
        return result;
    }

    public Task<IEnumerable<SongInfo>> GetTopPlay(DateTime? from, DateTime? to, int? top)
    {
        throw new NotImplementedException();   
        
    }
}
