using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.LocalStorage.Common;

public class LocalStorageSettings
{
    public string StoragePath { get; set; } = null!;
    public const string SectionName  = "LocalStorage";
}
