using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MusicApp.Domain.Common.Entities;
using MusicApp.Infrastructure.Common.Interface.Storage;
using MusicApp.Storage.Local.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Storage.Local.Persistence;

public class FileStorage : IFileStorage
{
    private readonly string _path;
    public FileStorage(IOptions<LocalStorageSettings> options)
    {
        _path = options.Value.StoragePath;
    }
    public Stream GetFile(string path, FileType fileType)
    {
        return File.OpenRead(Path.Combine(_path, fileType.ToString(), path));
    }

    public Task DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        return Task.CompletedTask;
    }


    public FileStream Upload(string path, FileType fileType)
    {
        return new FileStream(Path.Combine(_path, fileType.ToString(), path), FileMode.Create);
    }

    public async Task<string> UploadAsync(Stream file, FileType fileType, string path)
    {   
            await file.CopyToAsync(file);
            return path;
    } 
    public async Task DeleteAsync(string path)
    {
        await DeleteFile(path);
    }

    public Task<string> GetFileName(string path, FileType fileType)
    {
        return Task.FromResult(Path.GetFileName(Path.Combine(_path, fileType.ToString(), path)));
    }

    public Task<string> GetFilePath(string fileName, FileType fileType)
    {
        return Task.FromResult(Path.GetFileName(Path.Combine(_path, fileType.ToString())));
    }

    public Task<string> DownloadAsync(Stream file, FileType fileType, string fileName)
    {
        throw new NotImplementedException();
    }
}
