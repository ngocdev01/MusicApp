namespace MusicApp.Domain.Common.Entities;

public partial class Genre : EntityBase
{
    public string Name { get; set; } = null!;

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
