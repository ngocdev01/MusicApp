using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Contracts.Request;

namespace MusicApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;

        public AdminController(IAdminService adminService,IUserService userService)
        {
            _adminService = adminService;
            _userService = userService;
        }

        [HttpGet("play/year")]
        public async Task<IEnumerable<KeyValuePair<DateTime,int>>> GetChartData([FromQuery]GetTopPlayRequest request)
        {
            return await _adminService.GetPlayTimeChart(request.from, request.to);
        }
        [HttpGet("play/month")]
        public async Task<IEnumerable<KeyValuePair<DateTime, int>>> GetChartDataMonth(DateTime month)
        {
            return await _adminService.GetPlayTimeChart(month);
        }
        [HttpGet("user/all")]
        public async Task<IEnumerable<UserInfo>> GetAllUser()
        {
            return await _userService.GetAll();
        }

        [HttpPut("user/{id}")]
        public async Task<UserInfo> UpdateUser(string id, UserInfo user)
        {
            return await _userService.Update(id,user);
        }

        [HttpGet("role/all")]
        public async Task<IEnumerable<RoleInfo>> GetRoles()
        {
            return await _adminService.Roles();
        }

        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _userService.Delete(id);
            return NoContent();
        }
        [HttpPost("user")]
        public async Task<IActionResult> CreateUser(UserInfo userInfo)
        {
            await _userService.CreateUser(userInfo);
            return NoContent();       
        }
        [HttpGet]
        public AppInfomationResult GetInfo()
        {
            return _adminService.GetAppInfomationResult();
        }
        [HttpGet("train")]
        public async Task<ModelTrainResult> TrainModel(int clusterNumber)
        {
            return await _adminService.TrainModel(clusterNumber);
        }

    }
}
