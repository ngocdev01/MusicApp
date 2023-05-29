using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Services.DTOs.ObjectInfo;

public record  ArtistInfo(string Id,string Name,string? Image,string? Background)
{
    public ArtistInfo(Artist artist) : this(artist.Id, artist.Name, artist.Image, artist.BackGround) { }
};
