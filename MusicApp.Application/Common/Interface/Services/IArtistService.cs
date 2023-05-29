using Microsoft.AspNetCore.Http;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Common.Interface.Services;

public interface IArtistService
{
    
    Task<ArtistResult> GetArtist(string id);
    Task<ArtistResult> GetArtistByName(string name);
    Task<ArtistResult> GetArtist(Expression<Func<Artist,bool>> expression);
    Task<ArtistResult> CreateArtist(string name, IFormFile? image, IFormFile? background);
    Task<IEnumerable<ArtistResult>> GetAll();
    Task<IEnumerable<ArtistResult>> GetArtists(Expression<Func<Artist, bool>> expression);
}
