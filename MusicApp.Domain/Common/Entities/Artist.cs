namespace MusicApp.Domain.Common.Entities;
public partial class Artist : EntityBase
{
    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public string? BackGround { get; set; } = null!;

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
