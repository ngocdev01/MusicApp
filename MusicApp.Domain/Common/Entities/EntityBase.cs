using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.Common.Entities;

public abstract class EntityBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
}
