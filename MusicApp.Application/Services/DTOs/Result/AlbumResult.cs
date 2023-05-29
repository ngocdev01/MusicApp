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
    public AlbumResult(Album album)
    {
        Album = new AlbumInfo(album);
        Songs = album.Songs.Select(song => new SongResult(song)).ToList();
        Artists = album.Artists.Select(artist => new ArtistInfo(artist)).ToList();
    }
}


