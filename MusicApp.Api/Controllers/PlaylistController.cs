using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Contracts.Request;

namespace MusicApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpPost("create")]
        public IActionResult CreatePlaylist(CreatePlaylistRequest request)
        {
            _playlistService.AddPlaylist(request.playlistName.Trim(), request.ownerId);
            return Ok();
        }
        [HttpPost("createBySong")]
        public async Task<PlaylistInfo> CreatePlaylistBySong(CreatePlaylisBySongtRequest request)
        {
            return await _playlistService.AddPlayListBySong(request.ownerId, request.songId);

        }
        [HttpGet("{id}")]
        public async Task<PlaylistResult> GetPlaylist(string id)
        {
            return await _playlistService.GetPlaylist(id);

        }

        [HttpPost("update")]
        public async Task<PlaylistResult> UpdatePlayList([FromForm] UpdatePlaylistRequest request)
        {

            return await _playlistService.UpdatePlaylist(request.id, request.name, request.image);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(string id)
        {
            await _playlistService.RemovePlaylist(id);
            return Ok();
        }
        [HttpGet]
        public async Task<IEnumerable<PlaylistInfo>> GetPlaylistByOwner([FromQuery] string ownerId)
        {
            return await _playlistService.GetByOwner(ownerId);
        }
        [HttpGet("all")]
        public async Task<IEnumerable<PlaylistInfo>> GetAll()
        {
            return await _playlistService.GetAll();
        }

        [HttpPost("{id}")]
        public async Task<PlaylistResult> AddSongToPlaylist(string id, string songId)
        {
            return await _playlistService.AddSongToPlaylist(id, songId);
        }
        [HttpDelete("{id}/{songId}")]
        public async Task<IActionResult> DeletePlayListSong(string id, string songId)
        {
            await _playlistService.RemoveSongFromPlaylist(id, songId);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PlaylistPlayEvent(string id,string userId)
        {
            await _playlistService.PlaylistPlayEvent(id, userId);
            return Ok();
        }
    }
}
