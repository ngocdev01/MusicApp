using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Contracts.Request;

public record GetTopPlayRequest(DateTime from,DateTime to,int? param);
