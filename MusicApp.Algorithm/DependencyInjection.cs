using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MusicApp.Algorithm.Common;
using MusicApp.Algorithm.Persistence;
using MusicApp.Application.Common.Interface.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Algorithm;

public static class DependencyInjection
{
    public static IServiceCollection AddAlgorithm(this IServiceCollection services, ConfigurationManager configuration)
    {
        string? conString;
        if (configuration.GetValue<bool>("Storage:CloudMode"))
        {
            conString = configuration.GetConnectionString("Cloud");
        }
        else
        {
            conString = configuration.GetConnectionString("Local");
        }

        if (string.IsNullOrEmpty(conString))
            throw new ArgumentNullException("Connection String is NULL");
        DatabaseSourceSetting setting = new DatabaseSourceSetting();
        setting.ConnectionString = conString;
        services.AddSingleton(Options.Create(setting));
        services.AddScoped<IClustering, SongClustering>();
        return services;
    }
}
