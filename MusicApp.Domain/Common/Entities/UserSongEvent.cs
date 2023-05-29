using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.Common.Entities;

public class UserSongEvent : UserEvent
{
    public string SongId { get; set; } = null!;
    public virtual Song SongNavigation { get; set; } = null!;
}
