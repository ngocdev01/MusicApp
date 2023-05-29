using Microsoft.AspNetCore.Http;
using MusicApp.Application.Services;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Common.Interface.Services;

public interface ISongService
{
    public Task<IEnumerable<SongResult>> Recommend(params string[] id);
    public Task<SongResult> GetSongById(string id);
    public Task<SongResult> AddSong(Song song);
    public Task<SongResult> GetSongByName(string name);
    public Task<IEnumerable<SongResult>> GetAll();
    public Task DeleteSongById(string id);
    public Task UpdateSongName(string id,string name);
    public Task UpdateSongAlbum(string id,string albumId);
    public Task UpdateSongArtist(string id,string artistId);
    public Task UpdateSongCount(string id,int count);
    public Task<SongResult> CreateSong(string name,
                                       string album,
                                       string[] artist,
                                       string[]? genre,
                                       IFormFile audio);
    Task SongPlayEvent(string id, string userId);
    Task<IEnumerable<string>> GetUsers(string id);
    Task<IEnumerable<SongInfo>> GetTopPlay(DateTime? from, DateTime? to, int? top);
}
