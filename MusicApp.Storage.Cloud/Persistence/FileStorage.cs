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
using MusicApp.Domain.Common.Entities;

namespace MusicApp.Storage.Cloud.Persistence;

public class FileStorage : IFileStorage
{
    private readonly string? _path = "Data";
    private readonly string bucketName;
    private readonly StorageClient storageClient;
    private readonly GoogleCredential? credential;
    private readonly UrlSigner? urlSigner;
    public FileStorage(IOptions<CloudStorageSettings> options)
    {

        byte[] credentialsBytes = Convert.FromBase64String(options.Value.Credential);
        var json = Encoding.UTF8.GetString(credentialsBytes);
        bucketName = options.Value.Bucket;
        credential = GoogleCredential.FromJson(json);
        urlSigner = UrlSigner.FromCredential(credential);
        storageClient = StorageClient.Create(credential);

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

    public async Task<string> UploadAsync(Stream file,FileType fileType, string fileName)
    {
        await storageClient.UploadObjectAsync(bucketName,
                                              string.Concat(_path, "/", fileType.ToString().ToLower(), "/", fileName),
                                              null,
                                              file);
        return fileName;
    }
    public async Task<string> DownloadAsync(Stream file, FileType fileType, string fileName)
    {
        await storageClient.DownloadObjectAsync(bucketName,
                                                string.Concat(_path, "/", fileType.ToString().ToLower(), "/", fileName),
                                                file);
        return fileName;
    }
    public async Task DeleteAsync(string path)
    {
        var obj = await storageClient.GetObjectAsync(bucketName,path);
        await storageClient.DeleteObjectAsync(obj);
    }

    public IEnumerable<string> GetObjects()
    {
        foreach (var obj in storageClient.ListObjects("music-app-storage", ""))
        {
            yield return obj.Name;
        }
    }

    public async Task<string> GetFileName(string path, FileType fileType)
    {
        var file = await storageClient.GetObjectAsync(bucketName, String.Concat(_path, "/", fileType.ToString().ToLower(), "/", path));
        return file.Name;
    }

    public async Task<string> GetFilePath(string fileName, FileType fileType)
    {      
        return await Task.FromResult(String.Concat(_path, "/", fileType.ToString().ToLower(), "/", fileName));
    }
}
