using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Contracts.Request;
using MusicApp.Domain.Common.Entities;

namespace MusicApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;
        private readonly IFileRepository _fileRepository;

        public AlbumController(IAlbumService albumService,IFileRepository fileRepository)
        {
            _albumService = albumService;
            _fileRepository = fileRepository;
        }

        [HttpGet("{id}")]
        public async Task<AlbumResult> GetAlbum(string id)
        {
            return await _albumService.GetAlbum(id);
        }
        [HttpGet]
        public async Task<AlbumResult> GetAlbumByName([FromQuery]string name)
        {
            return await _albumService.GetAlbumByName(name.Trim());
        }
        [HttpGet("Top")]
      
        public async Task<IEnumerable<AlbumInfo>> GetTopAlbum([FromQuery] int top,[FromQuery]string? orderBy)
        {
            return await _albumService.GetTopAlbum(top,orderBy);
        }



        [HttpGet("all")]
        public async Task<IEnumerable<AlbumResult>> GetAll()
        {
            return await _albumService.GetAll();       
        }

        [HttpPost]
        public async Task<AlbumResult> CreateAlbum([FromForm]CreateAlbumRequest request)
        {       
            return await _albumService.CreateAlbum(request.name.Trim(), request.artists, request.image);
        }

        [HttpGet("topPlay")]
        public async Task<IEnumerable<AlbumInfo>> GetTopAlbumByPlay([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int? skip, [FromQuery] int? take)
        {
            return await _albumService.GetAlbumByPlayTime(from, to, skip, take); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AlbumPlayEvent(string id, string userId)
        { 
            await _albumService.AlbumPlayEvent(id, userId);
            return Ok();
        }


    }
}
