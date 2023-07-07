using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Api.Common.Attribute;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.Result;

namespace MusicApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ImageController : ControllerBase
{
    private readonly IFileRepository _fileRepository;
    private readonly IFileStorageService _fileStorageService;

    public ImageController( IFileRepository fileRepository,IFileStorageService fileStorageService)
    {   
        _fileRepository = fileRepository;
        _fileStorageService = fileStorageService;
    }

    [HttpGet("{id}")]
    [ServiceFilter(typeof(StorageMode))]
    public IActionResult GetImage(string id)
    {
        var fs = _fileRepository.GetImage(id);

        return File(fs,"image/png+jpg");
    }

    [HttpGet("upload")]
    public async Task<UploadURLResult> GetUploadUrl(string fileName, string contentType)
    {
        return await _fileStorageService.GetImageUploadUrl(fileName, contentType);
    }
}
