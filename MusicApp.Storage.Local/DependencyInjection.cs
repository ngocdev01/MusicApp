﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MusicApp.Application.Common.Interface.Authentication;
using MusicApp.Infrastructure.Authentication;
using MusicApp.Infrastructure.Common.Interface.Storage;
using MusicApp.Storage.Local.Common;
using MusicApp.Storage.Local.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Storage.Local;

public static class DependencyInjection
{
    public static IServiceCollection AddLocalStorage(this IServiceCollection services,ConfigurationManager configuration)
    {
        services.Configure<LocalStorageSettings>(configuration.GetSection(LocalStorageSettings.SectionName));
        services.AddSingleton<IFileStorage, FileStorage>();
        return services;
    }
}
