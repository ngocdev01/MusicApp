using Microsoft.AspNetCore.Http;

namespace MusicApp.Application.Common.Interface.Persistence;

public interface IFileRepository
{
    public FileStream GetAudio(string id);
    public FileStream GetImage(string id);

    public Task<string> UploadImageAsync(IFormFile file);
    public Task<string> UploadAudioAsync(IFormFile file);
    public Task DeleteAsync(string path);

}
