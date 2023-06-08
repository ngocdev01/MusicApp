using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Storage.Local.Common;

public class LocalStorageSettings
{
    public string StoragePath { get; set; } = null!;
    public const string SectionName  = "Storage:Local";
}
