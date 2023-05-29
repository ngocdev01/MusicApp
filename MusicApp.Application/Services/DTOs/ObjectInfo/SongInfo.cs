using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Services.DTOs.ObjectInfo;

public record SongInfo(string id,
    string name,
    string source,
    int count)
{
    public SongInfo(Song song) : this(song.Id, song.Name, song.Source, song.Count) { }
}
