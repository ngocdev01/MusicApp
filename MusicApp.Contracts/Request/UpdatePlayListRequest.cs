using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Contracts.Request;

public class UpdatePlaylistRequest
{
    public string? id { get; set; }
    public string? name { get; set; }
    public string? image { get; set; }
}
