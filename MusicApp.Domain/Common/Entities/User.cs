namespace MusicApp.Domain.Common.Entities;

public partial class User : EntityBase
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual ICollection<UserEvent> UserEvents { get; set; } = new List<UserEvent>();
    public virtual Role? RoleNavigation { get; set; } 
}
