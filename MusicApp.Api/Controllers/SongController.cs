using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Contracts.Request;
using MusicApp.Domain.Common.Entities;

namespace MusicApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class SongController : ControllerBase
{
    private readonly ISongService _songService;



    public SongController(ISongService songService)
    {
        _songService = songService;
    }



    [HttpGet("all")]
    public async Task<IEnumerable<SongResult>> GetSongs()
    {
        return await _songService.GetAll();
    }

    [HttpPut("recommned")]
    public async Task<IEnumerable<SongResult>> GetRecommend(params string[] id)
    {
        return await _songService.Recommend(id);
    }

    [HttpGet]
    public async Task<SongResult> GetSongByName([FromQuery] string name)
    {
        return await _songService.GetSongByName(name);
    }
    [HttpGet("{id}")]
    public async Task<SongResult> GetSong(string id)
    {
        return await _songService.GetSongById(id);
    }

    [HttpPost]
    public async Task<SongResult> AddSong([FromForm] CreateSongRequest request)
    {

        return await _songService.CreateSong(request.songName.Trim(),
                                             request.album,
                                             request.artists,
                                             request.genres,
                                             request.songAudio);

    }

    [HttpPut("{id}/play")]
    public async Task<IActionResult> SongPlayEvent(string id, string userId)
    {

        await _songService.SongPlayEvent(id, userId);
        return Ok();

    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSong(string id, UpdateSongRequest request)
    {

        await _songService.UpdateSong(id, request.songName, request.album, request.artists, request.genres, request.songAudio);
        return Ok();

    }


    [HttpGet("top")]
    public async Task<IEnumerable<SongInfo>> TopSong([FromQuery]GetTopPlayRequest query)
    {
        return await _songService.GetTopPlay(query.from, query.to, query.param);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSong(string id)
    {
        await _songService.DeleteSongById(id);
        return Ok();
    }
}
