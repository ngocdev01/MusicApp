using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Services.DTOs.Result;

public record PlaylistResult
{
    public PlaylistInfo Playlist { get; set; }
    public ICollection<SongResult> Songs { get; set; }

    public PlaylistResult(Playlist playlist)
    {
        Playlist = new PlaylistInfo(playlist);
        Songs = playlist.Songs.Select(song => new SongResult(song)).ToList();
    }
}
