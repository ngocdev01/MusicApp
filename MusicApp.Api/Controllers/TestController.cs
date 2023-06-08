using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Storage.Cloud.Persistence;

namespace MusicApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly FileStorage _cloudFileStorage;

        public TestController(FileStorage cloudFileStorage)
        {
            _cloudFileStorage = cloudFileStorage;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return Ok(_cloudFileStorage.GetObjects());
        }
    }
}
