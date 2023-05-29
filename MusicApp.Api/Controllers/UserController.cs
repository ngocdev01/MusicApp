using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;

namespace MusicApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<UserInfo> GetUserInfo(string id)
        {
            return await _userService.GetUser(id);
        }

        [HttpGet("{id}/songs")]
        public async Task<IEnumerable<SongResult>> GetBestSongInMonth(string id)
        {
            return await _userService.GetBestSongInMonth(id);
        }
        [HttpGet("{id}/albums")]
        public async Task<IEnumerable<AlbumInfo>> GetBestAlbumInMonth(string id)
        {
            return await _userService.GetBestAlbumInMonth(id);
        }
    }
}
