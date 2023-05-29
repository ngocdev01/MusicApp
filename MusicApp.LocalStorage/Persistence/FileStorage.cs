using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MusicApp.Infrastructure.Common;
using MusicApp.Infrastructure.Common.Interface.Storage;
using MusicApp.LocalStorage.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.LocalStorage.Persistence;

public class FileStorage : IFileStorage
{
    private readonly string _path;
    public FileStorage(IOptions<LocalStorageSettings> options)
    {
        _path = options.Value.StoragePath;
    }
    public FileStream GetFile(string path, FileType fileType)
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


    public FileStream Upload(string path,FileType fileType)
    {
        return new FileStream(Path.Combine(_path, fileType.ToString(), path), FileMode.Create);
    }

    public async Task<string> UploadAsync(IFormFile file,FileType fileType, string path)
    {
        using (var fs = Upload(path, fileType))
        {
            
            await file.CopyToAsync(fs);
            return path;
        }              
    }
    public async Task DeleteAsync(string path)
    {
        await DeleteFile(path);
    }
}
