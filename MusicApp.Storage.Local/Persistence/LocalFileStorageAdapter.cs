using Microsoft.AspNetCore.Hosting;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Storage.Local.Persistence;

public class LocalFileStorageAdapter : IFileStorageAdapter
{
    private readonly string domain;
    public LocalFileStorageAdapter(IWebHostEnvironment environment)
    {
        domain = environment.WebRootPath;
        int a = 1;
    }
    public string GetFileUrl(FileType fileType, string fileName, uint? time = null)
    {
        throw new NotImplementedException();
    }

    public string GetUpLoadUrl(FileType fileType,string fileName,uint? time = null)
    {
        throw new NotImplementedException();
    }
}
