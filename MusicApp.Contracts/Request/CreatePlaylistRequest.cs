using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Contracts.Request;

public record CreatePlaylisBySongtRequest( 
    string songId,
    string ownerId
);

