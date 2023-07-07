using Microsoft.AspNetCore.Http;
using MusicApp.Domain.Common.Entities;

namespace MusicApp.Application.Common.Interface.Persistence;

public interface IFileRepository
{
    public Stream GetAudio(string id);
    public Stream GetImage(string id);
    public Task<string> GetFileName(FileType fileType,string fileName);
    public Task<string> UploadImageAsync(IFormFile file);
    public Task<string> UploadAudioAsync(IFormFile file);
    public Task DeleteAsync(string path);
    public Task DeleteAsync(FileType fileType,string fileName);
    public Task<string> GetFilePath(FileType fileType,string fileName);
    public Task<string> UploadAsync(FileType fileType,Stream fileStream,string fileName);
    public Task<string> DownloadAsync(FileType fileType, Stream fileStream, string fileName);

}
