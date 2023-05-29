
namespace MusicApp.Contracts.Authencation;

public record AuthenticationResponse
(
    string id,
    string UserName,
    string Email,
    string Token,
    string? Role
);
