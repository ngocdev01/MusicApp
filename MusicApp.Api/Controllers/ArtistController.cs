using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Application.Services.Service;
using MusicApp.Contracts.Request;

namespace MusicApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {

        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet("{id}")]
        public async Task<ArtistResult> GetArtist(string id)
        {
            return await _artistService.GetArtist(id);
        }

        [HttpGet]
        public async Task<ArtistResult> GetArtistByName([FromQuery]string name)
        {
            return await _artistService.GetArtistByName(name.Trim());
        }
        [HttpGet("all")]
        public async Task<IEnumerable<ArtistResult>> GetAll()
        {
            return await _artistService.GetAll();

        }

        [HttpPost]
        public async Task<ArtistResult> CreateArtist([FromForm]CreateArtistRequest request)
        {
            return await _artistService.CreateArtist(request.name.Trim(), request.image, request.background);
        }
    }
}
