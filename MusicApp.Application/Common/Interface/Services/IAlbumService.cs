using Microsoft.AspNetCore.Http;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Common.Interface.Services;

public interface IAlbumService
{
    public Task<AlbumResult> GetAlbum(string id);
    public Task DeleteAlbum(string id);

    public Task<IEnumerable<AlbumInfo>> GetAll();
    public Task<AlbumResult> GetAlbumByName(string name);
    public Task<AlbumResult> CreateAlbum(string Name, string[]? artists, string? image);
    public Task AddAlbum(Album album);
    public Task<IEnumerable<AlbumResult>> GetSongsByAlbum(string albumId);
    public Task<IEnumerable<AlbumInfo>> GetTopAlbum(int top,string? orderBy);
    Task<IEnumerable<AlbumInfo>> GetAlbumByPlayTime(DateTime? from, DateTime? to, int? skip, int? take);
    Task AlbumPlayEvent(string id, string userId);
}
