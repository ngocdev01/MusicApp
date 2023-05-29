using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Application.Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Common.Interface.Services;

public interface IUserService 
{ 
    Task<UserInfo> GetUser(string id);
    Task<IEnumerable<AlbumInfo>> GetBestAlbumInMonth(string id);
    Task<IEnumerable<SongResult>> GetBestSongInMonth(string id);
    Task<IEnumerable<UserInfo>> GetAll();
    Task CreateUser(UserInfo userInfo);

    Task<UserInfo> Update(string id, UserInfo user);
    Task Delete(string id);
}
