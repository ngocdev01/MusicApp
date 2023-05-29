using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;

namespace MusicApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("Song")]
        public async Task<IEnumerable<SongResult>> SearchSong([FromQuery]string keyword,  [FromQuery] int? first, [FromQuery] int? skip)
        {
            return await _searchService.SearchSong(keyword,first,skip);
        }

        [HttpGet("Album")]
        public async Task<IEnumerable<AlbumInfo>> SearchAlbum([FromQuery]string keyword, [FromQuery] int? first, [FromQuery] int? skip)
        {
            return await _searchService.SearchAlbum(keyword, first, skip);

        }

        [HttpGet("Playlist")]
        public async Task<IEnumerable<PlaylistResult>> SearchPlaylist([FromQuery]string keyword, [FromQuery] int? first, [FromQuery] int? skip)
        {
            return await _searchService.SearchPLaylist(keyword, first, skip);
        }
        [HttpGet("Genre")]
        public async Task<IEnumerable<GenreInfo>> SearchGenre([FromQuery] string keyword, [FromQuery] int? first, [FromQuery] int? skip)
        {
            return await _searchService.SearchGenre(keyword, first, skip);
        }
        [HttpGet("Artist")]
        public async Task<IEnumerable<ArtistInfo>> SearchArtist([FromQuery] string keyword, [FromQuery] int? first, [FromQuery] int? skip)
        {
            return await _searchService.SearchArtist(keyword, first, skip);
        }
    }
}
