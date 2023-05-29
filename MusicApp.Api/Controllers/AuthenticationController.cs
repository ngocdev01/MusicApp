using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.Services.Authentication;
using MusicApp.Contracts.Authencation;


namespace MusicApp.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var authResult = await _authenticationService.Register(
            request.UserName,
            request.Email, 
            request.Password);
        var response = new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.UserName,
            authResult.User.Email,
            authResult.Token,
            authResult.User.RoleNavigation?.Name);
        return Ok(response);
    }

    [HttpPost("login")]
    public async  Task<IActionResult> Login(LoginRequest request)
    {
        var authResult = await _authenticationService.Login(
            request.Email,
            request.Email,
            request.Password);
        var response = new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.UserName,
            authResult.User.Email,
            authResult.Token,
            authResult.User.RoleNavigation?.Name);
        return Ok(response);
    }
}
