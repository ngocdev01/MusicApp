﻿using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Services.DTOs.Result;

public record SongResult
{
    public SongInfo Song { get; set; }
    public ICollection<ArtistInfo> Artists { get; set; }
    public ICollection<AlbumInfo> Albums { get; set; }
    public ICollection<GenreInfo> Genres { get; set; }
    public SongResult(Song song,IFileStorageAdapter adapter)
    {

        Song = new SongInfo(song,adapter);
        Artists = song.Artists.Select(artist => new ArtistInfo(artist)).ToList();
        Albums = song.Albums.Select(album => new AlbumInfo(album,adapter)).ToList();
        Genres = song.Genres.Select(gen => new GenreInfo(gen)).ToList();

    }
}