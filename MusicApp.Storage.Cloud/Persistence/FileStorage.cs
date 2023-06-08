using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MusicApp.Infrastructure.Common;
using MusicApp.Infrastructure.Common.Interface.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicApp.Storage.Cloud.Common;
using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;
using System.Text.Encodings.Web;
using System.Security.AccessControl;

namespace MusicApp.Storage.Cloud.Persistence;

public class FileStorage : IFileStorage
{
    private readonly string? _path = "Data";
    private readonly StorageClient storageClient;
    private readonly GoogleCredential? credential;
    private readonly UrlSigner? urlSigner;
    public FileStorage(IOptions<CloudStorageSettings> options)
    {
        
        var defaultCredential = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
        if (!string.IsNullOrEmpty(defaultCredential))
            storageClient = StorageClient.Create();
        else
        {
            byte[] credentialsBytes = Convert.FromBase64String(options.Value.Credential);
            var json = Encoding.UTF8.GetString(credentialsBytes);
            credential = GoogleCredential.FromJson(json);
            urlSigner = UrlSigner.FromCredential(credential);
            storageClient = StorageClient.Create(credential);
        }
    }
    public Stream GetFile(string path, FileType fileType)
    {
        var bucketName = "music-app-storage";
        var fileName = String.Concat(_path,"/", fileType.ToString().ToLower(), "/", path);
        var storageObject = storageClient.GetObject(bucketName, fileName);

        var stream = new MemoryStream();
        storageClient.DownloadObject(bucketName, fileName, stream);

        stream.Position = 0; // Reset the stream position to the beginning

        return stream;
    }
  

    public Task DeleteFile(string path)
    {
        throw new NotImplementedException();
    }


    public FileStream Upload(string path,FileType fileType)
    {
        throw new NotImplementedException();
    }

    public async Task<string> UploadAsync(IFormFile file,FileType fileType, string path)
    {
        throw new NotImplementedException();
    }
    public async Task DeleteAsync(string path)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetObjects()
    {
        foreach (var obj in storageClient.ListObjects("music-app-storage", ""))
        {
            yield return obj.Name;
        }
    }

}
