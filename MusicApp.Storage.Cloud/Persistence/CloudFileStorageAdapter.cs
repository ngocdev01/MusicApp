using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Domain.Common.Entities;
using MusicApp.Storage.Cloud.Common;
using System.IO;
using System.Text;

namespace MusicApp.Storage.Cloud.Persistence;
public class CloudFileStorageAdapter : IFileStorageAdapter
{
    private readonly string? _path = "Data";
    private readonly string bucketName;
    private readonly StorageClient storageClient;
    private readonly GoogleCredential credential;
    private readonly UrlSigner urlSigner;
    public CloudFileStorageAdapter(IOptions<CloudStorageSettings> options)
    {
        byte[] credentialsBytes = Convert.FromBase64String(options.Value.Credential);
        var json = Encoding.UTF8.GetString(credentialsBytes);
        bucketName = options.Value.Bucket;
        credential = GoogleCredential.FromJson(json);
        urlSigner = UrlSigner.FromCredential(credential);
        storageClient = StorageClient.Create(credential);
    }
    public string GetFileUrl(FileType fileType, string fileName,uint? time = null)
    {
        var filePath = String.Concat(_path, "/", fileType.ToString().ToLower(), "/", fileName);
        var url = urlSigner.Sign(bucketName,
                                 filePath,
                                 time.HasValue ? TimeSpan.FromMinutes(time.Value) : TimeSpan.FromMinutes(30),
                                 HttpMethod.Get);
        return url;
    }

    public string GetUpLoadUrl(FileType fileType, string fileName, uint? time = null)
    {
        var filePath = String.Concat(_path, "/", fileType.ToString().ToLower(), "/", fileName);
        var url = urlSigner.Sign(bucketName,
                                 filePath,
                                 time.HasValue ? TimeSpan.FromMinutes(time.Value) : TimeSpan.FromMinutes(30),
                                 HttpMethod.Put); ;
        return url;
    }
}
