using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Domain.Common.Entities;
using MusicApp.Domain.Common.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Services.Service;

public class FileStorageService : IFileStorageService
{
    private readonly IFileStorageAdapter _fileStorageAdapter;
    public FileStorageService(IFileStorageAdapter fileStorageAdapter)
    {
        _fileStorageAdapter = fileStorageAdapter;
    }

    public async Task<UploadURLResult> GetAudioUploadUrl(string fileName, string contentType)
    {
        if (!contentType.StartsWith("audio/"))
        {
            throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType, "File type need to be audio");
        }
        string newFileName = string.Concat(Guid.NewGuid(), Path.GetExtension(fileName));
        return await Task.FromResult(new UploadURLResult(newFileName, _fileStorageAdapter.GetUpLoadUrl(FileType.Audio, newFileName)));

    }
    public async Task<UploadURLResult> GetImageUploadUrl(string fileName, string contentType)
    {
        if (!contentType.StartsWith("image/"))
        {
            throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType, "File type need to be image");
        }
        string newFileName = string.Concat(Guid.NewGuid(), Path.GetExtension(fileName));
        return await Task.FromResult(new UploadURLResult(newFileName, _fileStorageAdapter.GetUpLoadUrl(FileType.Image, newFileName)));
    }
}



