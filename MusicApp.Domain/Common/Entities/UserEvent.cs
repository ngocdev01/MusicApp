using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.Common.Entities;

public abstract class UserEvent : EntityBase
{
    public string UserId { get; set; } = null!;
    public int EventType { get; set; } 
    public DateTime Time { get; set; }
    public virtual User UserNavigation { get; set; } = null!; 
    
}
