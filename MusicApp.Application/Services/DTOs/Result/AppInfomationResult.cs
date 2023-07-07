using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Services.DTOs.Result;

public record AppInfomationResult
(
    int songs,
    int albums,
    int artists,
    int platlists,
    int playTime,
    int users
);
