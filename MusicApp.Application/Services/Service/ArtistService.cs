using Microsoft.AspNetCore.Http;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Domain.Common.Entities;
using MusicApp.Domain.Common.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MusicApp.Application.Services.Service;

public class ArtistService : BaseService, IArtistService
{
    private readonly IRepository<Artist> _artistRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IFileStorageAdapter _fileStorageAdapter;

    public ArtistService(IRepository<Artist> artistRepository, IFileRepository fileRepository,IFileStorageAdapter fileStorageAdapter)
    {
        _artistRepository = artistRepository;
        _fileRepository = fileRepository;
        _fileStorageAdapter = fileStorageAdapter;
    }

    public async Task<ArtistResult> CreateArtist(string name, IFormFile? image, IFormFile? background)
    {
        Artist artist = new();
        artist.Id = Guid.NewGuid().ToString();
        artist.Name = name;

        Task<string> upImage = image is not null ?
            _fileRepository.UploadImageAsync (image) : Task.FromResult("");
        Task<string> upBackground = background is not null ?
            _fileRepository.UploadImageAsync(background) : Task.FromResult("");
        string[] res = Array.Empty<string>();
        try
        {
            res = await Task.WhenAll(upImage, upBackground);
            artist.Image = string.IsNullOrEmpty(res[0]) ? null: res[0];
            artist.BackGround = string.IsNullOrEmpty(res[1])?  null :  res[1] ;
            await _artistRepository.AddAsync(artist);
        }
        catch(Exception e)
        {
            foreach (var path in res)
                await _fileRepository.DeleteAsync(path);
            throw new HttpResponseException(System.Net.HttpStatusCode.Conflict, "Upload file error" + e.Message + e.InnerException);
        }

            
        return new ArtistResult(artist,_fileStorageAdapter);
    }

    public async Task<ArtistResult> GetArtist(string id)
    {
        var artist = await GetEntityAsync(_artistRepository, id);
        
  
        return new ArtistResult(artist,_fileStorageAdapter);
    }


    public async Task<IEnumerable<ArtistResult>> GetArtists(Expression<Func<Artist, bool>> expression)
    {
        var artists = await _artistRepository.WhereAsync(expression);
        return artists.Select(artist => new ArtistResult(artist,_fileStorageAdapter)); 
    }
    public async Task<IEnumerable<ArtistResult>> GetAll()
    {
        var artists =  await _artistRepository.GetAll();
        return artists.Select(artist => new ArtistResult(artist, _fileStorageAdapter));
    }

    public async Task<ArtistResult> GetArtist(Expression<Func<Artist, bool>> expression)
    {
        ;
        return new ArtistResult(await GetEntityAsync(_artistRepository, expression),_fileStorageAdapter);
    }

    public async Task<ArtistResult> GetArtistByName(string name)
    { 
        return new ArtistResult(await GetEntityAsync(_artistRepository,artist => artist.Name == name),_fileStorageAdapter);
    }
}
