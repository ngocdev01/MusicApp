using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.Service;
using MusicApp.Infrastructure.Common.Interface.Storage;
using MusicApp.Storage.Cloud.Common;
using MusicApp.Storage.Cloud.Persistence;

namespace MusicApp.Storage.Cloud;

public static class DependencyInjection
{
    public static IServiceCollection AddCloudStorage(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<CloudStorageSettings>(configuration.GetSection(CloudStorageSettings.SectionName));
       
        services.AddSingleton<IFileStorageAdapter,CloudFileStorageAdapter>();
        services.AddSingleton<IFileStorage, FileStorage>();
        return services;
    }
}
