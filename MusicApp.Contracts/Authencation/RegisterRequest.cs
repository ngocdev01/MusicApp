namespace MusicApp.Contracts.Authencation;
public record RegisterRequest
(
    string UserName,
    string Email,
    string Password
);
