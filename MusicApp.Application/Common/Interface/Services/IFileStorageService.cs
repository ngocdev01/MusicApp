using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Domain.Common.Entities;
using MusicApp.Domain.Common.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Common.Interface.Services;

public interface IFileStorageService
{
    public Task<UploadURLResult> GetAudioUploadUrl(string fileName, string contentType);
    public Task<UploadURLResult> GetImageUploadUrl(string fileName, string contentType);
}
