﻿using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Api.Common.Attribute;
using MusicApp.Application.Common.Interface.Persistence;


namespace MusicApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(StorageMode))]
public class ImageController : ControllerBase
{
    private readonly IFileRepository _fileRepository;

    public ImageController( IFileRepository fileRepository)
    {   
        _fileRepository = fileRepository;
    }

    [HttpGet("{id}")]
    public IActionResult GetImage(string id)
    {
        var fs = _fileRepository.GetImage(id);

        return File(fs,"image/png+jpg");
    }
}
