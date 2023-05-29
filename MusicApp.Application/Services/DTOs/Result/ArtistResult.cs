using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Services.DTOs.Result;

public record ArtistResult
{
    public ArtistInfo Artist { get; set; }
    public ICollection<SongInfo> Songs { get; set; }
    public ICollection<AlbumInfo> Albums { get; set; }
    public ArtistResult(Artist artist)
    {
        Artist = new ArtistInfo(artist);
        Songs = artist.Songs.Select(song => new SongInfo(song)).ToList();
        Albums = artist.Albums.Select(album => new AlbumInfo(album)).ToList();
    }
}
