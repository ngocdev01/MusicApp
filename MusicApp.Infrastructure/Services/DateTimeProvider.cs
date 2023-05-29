using MusicApp.Application.Common.Interface.Services;

namespace MusicApp.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
