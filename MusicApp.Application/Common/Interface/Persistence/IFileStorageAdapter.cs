using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Common.Interface.Persistence;

public interface IFileStorageAdapter
{
    string GetFileUrl(FileType fileType, string fileName, uint? time = null);
    string GetUpLoadUrl(FileType fileType,string fileName, uint? time = null);
}
