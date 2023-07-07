

using Microsoft.Extensions.DependencyInjection;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.Authentication;
using MusicApp.Application.Services.Service;

namespace MusicApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ISongService, SongService>();
        services.AddScoped<IPlaylistService, PlaylistService>();
        services.AddScoped<IAlbumService, AlbumService>();
        services.AddScoped<IPlaylistService, PlaylistService>();
        services.AddScoped<IArtistService, ArtistService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<ISearchService, SearchService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<IFileStorageService, FileStorageService>();
        return services;
    }
}
