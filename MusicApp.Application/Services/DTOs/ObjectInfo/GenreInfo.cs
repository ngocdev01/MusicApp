using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Services.DTOs.ObjectInfo;

public record GenreInfo(string id,string name)
{
    public GenreInfo(Genre genre) : this(genre.Id, genre.Name) { }
}
