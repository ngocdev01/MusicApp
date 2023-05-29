using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Common.Interface.Services;

public interface ISearchService
{
    public Task<IEnumerable<SongResult>> SearchSong(string keyword, int? take = null, int? skip = null);
    public  Task<IEnumerable<PlaylistResult>> SearchPLaylist(string keyword, int? take = null, int? skip = null);
    public  Task<IEnumerable<ArtistInfo>> SearchArtist(string keyword, int? take = null, int? skip = null);
    public Task<IEnumerable<AlbumInfo>> SearchAlbum(string keyword, int? take = null, int? skip = null);
    public Task<IEnumerable<GenreInfo>> SearchGenre(string keyword, int? take = null, int? skip = null);

}
