namespace MusicApp.Domain.Common.Entities;
public partial class Playlist: EntityBase
{
    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public string? Owner { get; set; }


    public virtual User? OwnerNavigation { get; set; }

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();

    public virtual ICollection<UserPlaylistEvent> UserPlaylistEvents { get; set; } = new List<UserPlaylistEvent>();
}
