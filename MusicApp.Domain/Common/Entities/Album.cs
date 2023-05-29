

namespace MusicApp.Domain.Common.Entities;

public partial class Album : EntityBase
{
    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();

    public virtual ICollection<UserAlbumEvent> UserAlbumEvents { get; set; } = new List<UserAlbumEvent>();
}
