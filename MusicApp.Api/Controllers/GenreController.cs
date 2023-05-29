using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.ObjectInfo;

namespace MusicApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpPost]
        public async Task<GenreInfo> CreateGenre(string name)
        {
            return await _genreService.Create(name.Trim());
        }

        [HttpGet("all")]
        public async Task<IEnumerable<GenreInfo>> GetAllGenre()
        {
            return await _genreService.GetAll();
        }

        [HttpGet]
        public async Task<GenreInfo> GetByName([FromQuery] string name)
        {
            return await _genreService.GetByName(name.Trim());
        }
        [HttpGet("{id}")]
        public async Task<GenreInfo> Get(string id)
        {
            return await _genreService.Get(id);
        }


    }
}