namespace MusicApp.Domain.Common.Entities;

public partial class Song : EntityBase
{

    public string Name { get; set; } = null!;

    public string Source { get; set; } = null!;

    public int Count { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual ICollection<UserSongEvent> UserSongEvents { get; set; } = new List<UserSongEvent>();
}
