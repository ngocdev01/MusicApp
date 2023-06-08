using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Infrastructure.Common.Interface.Storage;

public interface IFileStorage
{
    Task DeleteAsync(string path);
    Task DeleteFile(string path);
    Stream GetFile(string path, FileType fileType);
    FileStream Upload(string path, FileType fileType);
    Task<string> UploadAsync(IFormFile file, FileType fileType, string path);
    
}
