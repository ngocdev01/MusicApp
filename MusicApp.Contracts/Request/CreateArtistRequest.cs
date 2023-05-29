using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Contracts.Request;

public record CreateArtistRequest(string name,IFormFile? image,IFormFile? background);
