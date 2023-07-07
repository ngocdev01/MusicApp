
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Services.DTOs.ObjectInfo;

public record PlaylistInfo( string Id, string Name,string? Owner,string? Image)
{
    public PlaylistInfo(Playlist playlist,IFileStorageAdapter adapter) : this(playlist.Id, playlist.Name, playlist.Owner,GetPlayListImage(playlist,adapter)) { }
    public static string? GetPlayListImage(Playlist playlist,IFileStorageAdapter adapter)
    {
        string? url = null;
        if(playlist.Image == null)
        {
            if (playlist.Songs.Count <= 0) return null;
            var song = playlist.Songs.FirstOrDefault(s => s.Albums.Count > 0);
            url =  song != null? song.Albums.ElementAtOrDefault(0)?.Image : null;
        }
        else url = playlist.Image;
        return string.IsNullOrEmpty(url) ? null : adapter.GetFileUrl(FileType.Image,url);
    }
}
