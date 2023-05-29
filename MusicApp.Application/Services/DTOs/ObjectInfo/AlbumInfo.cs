﻿using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Services.DTOs.ObjectInfo;

public record AlbumInfo(string Id,string Name,string? Image)
{
    public AlbumInfo(Album album) : this(album.Id, album.Name, album.Image) { }
};
