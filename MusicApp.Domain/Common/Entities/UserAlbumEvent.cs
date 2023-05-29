using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.Common.Entities;

public class UserAlbumEvent : UserEvent
{
    public string AlbumId { get; set; } = null!;
    public virtual Album AlbumNavigation { get; set; } = null!;
}
