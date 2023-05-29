using Microsoft.AspNetCore.Http;
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

public class PlaylistService :BaseService, IPlaylistService
{
    private readonly IRepository<Playlist> _playlistRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Song> _songRepository;
  

    public PlaylistService(IRepository<Playlist> playlistRepository,
                           IFileRepository fileRepository,
                           IRepository<User> userRepository,
                           IRepository<Song> songRepository)
    {
        _playlistRepository = playlistRepository;
        _fileRepository = fileRepository;
        _userRepository = userRepository;
        _songRepository = songRepository;
    }

    public async Task AddPlaylist(string name, string ownerId)
    {
        if ( await _userRepository.GetAsync(ownerId) is not User user)
        {
            throw new HttpResponseException(System.Net.HttpStatusCode.NotFound, "User not Exists");
        }


        var platList = new Playlist()
        {
            Id = Guid.NewGuid().ToString(),
            OwnerNavigation = user,
            Name = name,
        };
    }

    public async Task<PlaylistInfo> Create(string name, string ownerId, IFormFile? file)
    {
        throw new NotImplementedException();
    }

    public async Task<PlaylistInfo> AddPlayListBySong(string ownerId, string songId)
    {
        var user =await GetEntityAsync(_userRepository,ownerId);
        var song =await GetEntityAsync(_songRepository, songId);

        var platList = new Playlist()
        {
            Id = Guid.NewGuid().ToString(),
            Owner =user.Id,
            Name = song.Name,
            Songs = new List<Song>() { song },
        };

        await _playlistRepository.AddAsync(platList);
        return new PlaylistInfo(platList);
    }

    public async Task RemovePlaylist(string id)
    {
        if (await _playlistRepository.GetAsync(id) is not Playlist playlist)
        {
            throw new HttpResponseException(System.Net.HttpStatusCode.NotFound, "Playlist not Exists");
        }
        var c =  _playlistRepository.RemoveAsync(playlist);
        
    }
    Task IPlaylistService.SetPlayListImage(string name, string ownerId)
    {
        throw new NotImplementedException();
    }



    public async Task<PlaylistResult> GetPlaylist(string id)
    {
        var playlist = await GetEntityAsync(_playlistRepository, id);
        return new PlaylistResult(playlist);
    }

    public async Task<IEnumerable<PlaylistResult>> GetByOwner(string id)
    {
        var user = await GetEntityAsync(_userRepository,id);
        List < PlaylistResult > playlists = new();
        foreach(var playlist in user.Playlists)
        {
            playlists.Add(new PlaylistResult(playlist));
        }
        return playlists;
    }

    public async Task<PlaylistResult> UpdatePlaylist(string id, string name, IFormFile? image)
    {
        var playlist = await GetEntityAsync(_playlistRepository, id);
        if (image!=null)
        {
            if (!string.IsNullOrEmpty(playlist.Image))
            {
                await _fileRepository.DeleteAsync(playlist.Image);
            }

            var newImage = await _fileRepository.UploadImageAsync(image);
            await _playlistRepository.
            UpdateAsync(playlist, playlist => playlist.Image = newImage);
        }
        await _playlistRepository.
            UpdateAsync(playlist,playlist =>  playlist.Name = name );
        return new PlaylistResult(playlist);

    }

    public async Task<IEnumerable<PlaylistResult>> GetAll()
    {
        var playlists = await _playlistRepository.GetAll();
        List<PlaylistResult> results = new List<PlaylistResult>();
        foreach (var playlist in playlists)
        {
            results.Add( new PlaylistResult(playlist));
        }
        return results;
    }

    public async Task<PlaylistResult> AddSongToPlaylist(string playlistId, string songId)
    {
        var playlist =await GetEntityAsync(_playlistRepository, playlistId);
        var song =await GetEntityAsync(_songRepository, songId);

        await _playlistRepository.UpdateAsync(playlist,playlist => playlist.Songs.Add(song));
        return new PlaylistResult(playlist);
    }

    public async Task PlaylistPlayEvent(string id, string userId)
    {
        var user = await GetEntityAsync(_userRepository, userId);
        var playlist = await GetEntityAsync(_playlistRepository, id);
        await _playlistRepository.UpdateAsync(playlist, p => p.UserPlaylistEvents.Add(new UserPlaylistEvent()
        {
            PlaylistId = playlist.Id,
            UserId = user.Id,
            Time = DateTime.Now,
        }));
    }
}

