namespace MusicApp.Application.Services.Authentication;

public interface IAuthenticationService
{
    Task<AuthenticationResult> Register(string username,string email, string password);
    Task<AuthenticationResult> Login(string username,string email ,string password);
}