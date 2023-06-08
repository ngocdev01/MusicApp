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
    Task<IEnumerable<SongResult>> Recommend(params string[] id);
    Task<SongResult> GetSongById(string id);
    Task<SongResult> AddSong(Song song);
    Task<SongResult> GetSongByName(string name);
    Task<IEnumerable<SongResult>> GetAll();
    Task DeleteSongById(string id);
    Task UpdateSongCount(string id,int count);
    Task<SongResult> CreateSong(string name,
                                       string album,
                                       string[] artist,
                                       string[]? genre,
                                       IFormFile audio);
    Task UpdateSong(string id,
                    string name,
                    string album,
                    string[] artist,
                    string[]? genre,
                    IFormFile? audio);
    Task SongPlayEvent(string id, string userId);
    Task<IEnumerable<string>> GetUsers(string id);
    Task<IEnumerable<SongInfo>> GetTopPlay(DateTime? from, DateTime? to, int? top);
}
