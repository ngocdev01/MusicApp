using MusicApp.Domain.Common.Entities;


namespace MusicApp.Application.Common.Interface.Authentication;

public interface IJwtTokentGenerator
{
    public string GenerateToken(User user);
}
