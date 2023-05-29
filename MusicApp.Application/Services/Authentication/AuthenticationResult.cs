
using MusicApp.Domain.Common.Entities;

namespace MusicApp.Application.Services.Authentication;

public record AuthenticationResult
(   
    User User,
    string Token
);
