using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.Common.Entities;

public class UserPlaylistEvent : UserEvent
{
    public string PlaylistId { get; set; } = null!;
    public virtual Playlist PlaylistNavigation { get; set; } = null!;
}
