using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Api.Common.Attribute;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Contracts.Request;
using MusicApp.Infrastructure.DBContext;

namespace MusicApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(StorageMode))]
public class AudioController : ControllerBase
{
    //private readonly MusicContext _musicContext;
    private readonly IFileRepository _fileRepository;

    public AudioController( IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    [HttpGet("{id}")]
    public  IActionResult GetAudio(string id)
    {
       /* var song = await _musicContext.Songs.FindAsync(id);
        if (song == null)
        {
            return NotFound();
        }*/
        var fs = _fileRepository.GetAudio(id);
        var response = new FileStreamResult(fs, "audio/mp3");
        response.EnableRangeProcessing = true;
        Response.Headers.Add("Accept-Ranges", "bytes");
        Response.Headers.Add("Content-Range", $"{fs.Position}-{fs.Length}/{fs.Length}");
        return response;
    }
   
}
