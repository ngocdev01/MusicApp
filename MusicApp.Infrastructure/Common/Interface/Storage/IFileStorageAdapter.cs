using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Infrastructure.Common.Interface.Storage;

public interface IFileStorageAdapter
{
    string GetFileUrl(FileType fileType,string fileName);
    string GetUpLoadUrl(FileType fileType);


}
