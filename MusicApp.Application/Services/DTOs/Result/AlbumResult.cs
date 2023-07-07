using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Services.DTOs.Result;

public record AlbumResult
{
    public AlbumInfo Album { get; set; }
    public ICollection<ArtistInfo> Artists { get; set; }
    public ICollection<SongResult> Songs { get; set; }
    public AlbumResult(Album album,IFileStorageAdapter adapter)
    {
        Album = new AlbumInfo(album,adapter);
        Songs = album.Songs.Select(song => new SongResult(song,adapter)).ToList();
        Artists = album.Artists.Select(artist => new ArtistInfo(artist)).ToList();
    }
}


