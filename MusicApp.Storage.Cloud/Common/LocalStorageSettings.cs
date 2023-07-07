using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Storage.Cloud.Common;

public class CloudStorageSettings
{
    public string Credential { get; set; } = null!;
    public string Bucket { get; set; } = null!;
    public const string SectionName  = "Storage:Cloud";
}
