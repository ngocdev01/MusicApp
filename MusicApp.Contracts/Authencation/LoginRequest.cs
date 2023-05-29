namespace MusicApp.Contracts.Authencation;

public record LoginRequest
(
    string Email,
    string Password
);

