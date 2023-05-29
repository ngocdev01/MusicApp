using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Infrastructure.Common;
using MusicApp.Infrastructure.Common.Interface.Storage;
using System.Drawing;
using System.Drawing.Imaging;


namespace MusicApp.Infrastructure.Persistence;

public class FileRepository : IFileRepository
{
    IFileStorage _storage;
    public FileRepository(IFileStorage fileStorage)
    {
        _storage = fileStorage;
    }
    public FileStream GetAudio(string id)
    {
        return _storage.GetFile(id,FileType.Audio);
    }

    public FileStream GetImage(string id)
    {
        return _storage.GetFile(id, FileType.Image);
    }

    public Task<string> UploadImageAsync(IFormFile file)
    {
        string guid = Guid.NewGuid().ToString();
        var path  = guid + Path.GetExtension(file.FileName);

        return _storage.UploadAsync(file,FileType.Image, path);
    }
    public Task<string> UploadAudioAsync(IFormFile file)
    {
        string guid = Guid.NewGuid().ToString();
        var path = guid + Path.GetExtension(file.FileName);
        return _storage.UploadAsync(file,FileType.Audio, path);  
    }

    public async Task DeleteAsync(string path)
    {
        await _storage.DeleteAsync(path);
    }
}
