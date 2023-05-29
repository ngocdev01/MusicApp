using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Services.DTOs.ObjectInfo;

public record RoleInfo(string Id, string Name)
{
    public RoleInfo(Role role) : this(role.Id, role.Name) { }
}
