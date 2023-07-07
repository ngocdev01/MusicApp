using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Domain.Common.Entities;
using System.Linq;
using System.Net;
using MusicApp.Application.Services.DTOs.Result;
using Microsoft.AspNetCore.Http;

using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Domain.Common.Errors;
using static System.Net.Mime.MediaTypeNames;
using MusicApp.Application.Common.Interface.Algorithm;

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
    private readonly IFileStorageAdapter _fileStorageAdapter;
    private readonly IClustering _clustering;
    public SongService(IRepository<Song> songRepository,
                       IRepository<Album> albumRepository,
                       IRepository<Genre> genreRepository,
                       IRepository<Artist> artistRepository,
                       IFileRepository fileRepository,
                       IRepository<User> userRepository,
                       IRepository<UserSongEvent> userEventRepository,
                       IFileStorageAdapter fileStorageAdapter,
                       IClustering clustering)
    {
        _songRepository = songRepository;
        _albumRepository = albumRepository;
        _genreRepository = genreRepository;
        _artistRepository = artistRepository;
        _fileStorageAdapter = fileStorageAdapter;
        _userRepository = userRepository;
        _fileRepository = fileRepository;
        _userEventRepository = userEventRepository;
        _clustering = clustering;
    }

    public async Task<SongResult> AddSong(Song song)
    {
        await _songRepository.AddAsync(song);
        return new SongResult(song, _fileStorageAdapter);
    }

    public async Task DeleteSongById(string id)
    {
        var song = await GetEntityAsync(_songRepository, id);
        var audioSource = await _fileRepository.GetFilePath(FileType.Audio, song.Source);
        await _songRepository.RemoveAsync(id);
        await _fileRepository.DeleteAsync(audioSource);

    }

    public async Task<IEnumerable<SongResult>> GetAll()
    {
        var query = _songRepository.GetQueryInclude(s => s.Albums, s => s.Playlists, s => s.Genres, s => s.Artists);
        var list = await _songRepository.GetListAsync(query);
        return list.Select(song => new SongResult(song, _fileStorageAdapter));
    }



    public async Task UpdateSongCount(string id, int count)
    {
        var song = await GetEntityAsync(_songRepository, id);
        await _songRepository.UpdateAsync(song, song => song.Count++);
    }





    public async Task<SongResult> GetSongById(string id)
    {
        var song = await GetEntityAsync(_songRepository, id);
        return new SongResult(song, _fileStorageAdapter);
    }



    public async Task<SongResult> CreateSong(string name,
                                       string album,
                                       string[] artists,
                                       string[]? genres,
                                       string audio)
    {
        try
        {
            var audioSource = await _fileRepository.GetFilePath(FileType.Audio, audio);
            try
            {

                var albumList = new List<Album>() { await GetEntityAsync(_albumRepository, album) };
                List<Artist> artistList = new();
                foreach (var artist in artists)
                {
                    artistList.Add(await GetEntityAsync(_artistRepository, artist));
                }



                Song newSong = new Song()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    Source = audio,
                    Artists = artistList,
                    Albums = albumList,
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

                return new SongResult(newSong, _fileStorageAdapter);
            }
            catch (Exception e)
            {
                await _fileRepository.DeleteAsync(audioSource);
                throw new HttpResponseException(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        catch (Exception e)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, e.Message);
        }


    }

    public async Task<SongResult> GetSongByName(string name)
    {
        var song = await GetEntityAsync(_songRepository, s => s.Name == name);
        return new SongResult(song, _fileStorageAdapter);
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
        var songs = await _clustering.GetClusters(id);
        List<SongResult> result = new List<SongResult>();
        foreach (var i in songs)
            result.Add(new SongResult(await GetEntityAsync(_songRepository, i), _fileStorageAdapter));
        return result;
    }

    public Task<IEnumerable<SongInfo>> GetTopPlay(DateTime? from, DateTime? to, int? top)
    {
        throw new NotImplementedException();

    }

    public async Task UpdateSong(string id, string name, string album, string[] artists, string[]? genres, string? audio)
    {

        try
        {
            var songAudio = audio != null ? await _fileRepository.GetFilePath(FileType.Audio, audio) : null;

            try
            {

                var song = await GetEntityAsync(_songRepository, id);
                var songAlbum = await GetEntityAsync(_albumRepository, album);

                List<Artist> artistList = new();
                foreach (var artist in artists)
                {
                    artistList.Add(await GetEntityAsync(_artistRepository, artist));
                }
                if (genres != null)
                {
                    List<Genre> genreList = new();
                    foreach (var genre in genres)
                    {
                        genreList.Add(await GetEntityAsync(_genreRepository, genre));
                    }
                    song.Genres = genreList;
                }

                song.Name = name;
                song.Artists = artistList;
                song.Albums = new List<Album>() { songAlbum };

                if (audio != null)
                {
                    if (!string.IsNullOrEmpty(song.Source))
                    {
                        await _fileRepository.DeleteAsync(await _fileRepository.GetFilePath(FileType.Audio, song.Source));
                    }


                    await _songRepository.
                    UpdateAsync(song, song => song.Source = audio);
                }
            }
            catch (Exception e)
            {
                if (songAudio != null)
                    await _fileRepository.DeleteAsync(songAudio);
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, e.Message);
            }
        }
        catch (Exception e)
        {
            throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, e.Message);
        }
    }

}