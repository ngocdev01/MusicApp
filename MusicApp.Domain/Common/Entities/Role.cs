namespace MusicApp.Domain.Common.Entities;
public partial class Role : EntityBase
{ 

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
