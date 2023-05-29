
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicApp.Application.Common.Interface.Authentication;
using MusicApp.Application.Common.Interface.Authenticationl;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Domain.Common.Entities;
using MusicApp.Infrastructure.Authentication;
using MusicApp.Infrastructure.DBContext;
using MusicApp.Infrastructure.Persistence;
using MusicApp.Infrastructure.Services;
using System.Text;

namespace MusicApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<MusicContext>(
            options => options.UseLazyLoadingProxies().UseSqlServer("Data Source=(local)\\SQLEXPRESS;Initial " +
                              "Catalog=Music;Integrated Security=True;TrustServerCertificate=True"));
      
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddRepository();
        services.AddAuth(configuration);
        return services;
    }
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IRepository<User>, RepositoryBase<User>>();
        services.AddScoped<IRepository<Song>, RepositoryBase<Song>>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IRepository<Album>, RepositoryBase<Album>>();
        services.AddScoped<IRepository<Playlist>, RepositoryBase<Playlist>>();
        services.AddScoped<IRepository<Artist>,RepositoryBase<Artist>>();
        services.AddScoped<IRepository<Genre>, RepositoryBase<Genre>>();
        services.AddScoped<IRepository<Role>, RepositoryBase<Role>>();
        services.AddScoped<IRepository<UserSongEvent>, RepositoryBase<UserSongEvent>>();
        services.AddScoped<IRepository<UserAlbumEvent>, RepositoryBase<UserAlbumEvent>>();
        services.AddScoped<IRepository<UserPlaylistEvent>, RepositoryBase<UserPlaylistEvent>>();
        return services;
    }
    public static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
    {
        var JwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, JwtSettings);

        services.AddSingleton(Options.Create(JwtSettings));
        services.AddSingleton<IJwtTokentGenerator, JwtTokenGenerator>();
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters =
            new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = JwtSettings.Issuer,
                ValidAudience = JwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Secret)),
            });
        return services;
    }
}