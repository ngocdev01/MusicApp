using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Domain.Common.Entities;
using MusicApp.Infrastructure.Common.Interface.Storage;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace MusicApp.Infrastructure.Persistence;

public class FileRepository : IFileRepository
{
    IFileStorage _storage;
    public FileRepository(IFileStorage fileStorage)
    {
        _storage = fileStorage;
    }
    public Stream GetAudio(string id)
    {
        return _storage.GetFile(id,FileType.Audio);
    }

    public Stream GetImage(string id)
    {
        return _storage.GetFile(id, FileType.Image);
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        using (var ms = new MemoryStream())
        {
            string guid = Guid.NewGuid().ToString();
            var path = guid + Path.GetExtension(file.FileName);
            await file.CopyToAsync(ms);
            return await _storage.UploadAsync(ms,FileType.Image, path);
        }
    }
    public async Task<string> UploadAudioAsync(IFormFile file)
    {
        using (var ms = new MemoryStream())
        {
            string guid = Guid.NewGuid().ToString();
            var path = guid + Path.GetExtension(file.FileName);
            await file.CopyToAsync(ms);
            return await _storage.UploadAsync(ms, FileType.Audio, path);
        }
    }

    public async Task DeleteAsync(string path)
    {
        await _storage.DeleteAsync(path);
    }

    public Task<string> GetFileName(FileType fileType, string fileName)
    {
        return _storage.GetFileName(fileName,fileType);
    }

    public async Task DeleteAsync(FileType fileType, string fileName)
    {
       var path = await _storage.GetFilePath(fileName,fileType);
       await _storage.DeleteAsync(path);

    }

    public async Task<string> GetFilePath(FileType fileType, string fileName)
    {
        return await _storage.GetFilePath(fileName, fileType);
    }

    public async Task<string> UploadAsync(FileType fileType,Stream fileStream, string fileName)
    {
        return await _storage.UploadAsync(fileStream, fileType, fileName);
    }

    public async Task<string> DownloadAsync(FileType fileType, Stream fileStream, string fileName)
    {
        return await _storage.DownloadAsync(fileStream, fileType, fileName);
    }
}
