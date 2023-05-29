
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Discovery;


namespace MusicApp.GoogleDriveApi
{
    public class Class1
    {
        void Getfile()
        {
            var services = new BaseClientService.Initializer()
            {
                ApplicationName = "MusicApp",
                ApiKey = "AIzaSyDkHFyAGWrlZ4EPRFL8ZhpcTNhbsdzQ3No"
            };
            var driveService = new DriveService(services);
            FilesResource.ListRequest listRequest = driveService.Files.List();
        }
    }
}