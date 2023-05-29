using Microsoft.AspNetCore.Http;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Common.Interface.Services;

public interface IPlaylistService
{
    
    Task AddPlaylist(string name, string ownerId);
    Task<PlaylistInfo> Create(string name, string ownerId,IFormFile? file);
    Task<PlaylistResult> GetPlaylist(string id);
    Task<IEnumerable<PlaylistResult>> GetByOwner(string id);
    Task RemovePlaylist(string id);
    Task<PlaylistResult> UpdatePlaylist(string id, string name,IFormFile? image);
    Task<PlaylistInfo> AddPlayListBySong(string ownerId,string songId);
    Task SetPlayListImage(string name, string ownerId);
    Task<IEnumerable<PlaylistResult>> GetAll();
    Task<PlaylistResult> AddSongToPlaylist(string playlistId, string songId);
    Task PlaylistPlayEvent(string id, string userId);
}
