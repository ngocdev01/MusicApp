using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;
using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MusicApp.Application.Services.Service;

public class SearchService : ISearchService
{
    IRepository<Song> _songRepository;
    IRepository<Artist> _artistRepository;
    IRepository<Album> _albumRepository;
    IRepository<Playlist> _playlistRepository;
    IRepository<Genre> _genreRepository;

    public SearchService(IRepository<Song> songRepository,
                         IRepository<Artist> artistRepository,
                         IRepository<Album> albumRepository,
                         IRepository<Playlist> playlistRepository,
                         IRepository<Genre> genreRepository)
    {
        _songRepository = songRepository;
        _artistRepository = artistRepository;
        _albumRepository = albumRepository;
        _playlistRepository = playlistRepository;
        _genreRepository = genreRepository;
    }

    public async Task<IEnumerable<SongResult>> SearchSong(string keyword, int? take = null, int? skip = null)
    {
        var songs = await _songRepository
          .WhereSkipTakeAsync(album => album.Name.Contains(keyword),
          skip != null ? (int)skip : 0, take != null ? (int)take : 100);


        return songs.Select(a => new SongResult(a));
    }
    public async Task<IEnumerable<AlbumInfo>> SearchAlbum(string keyword,int? take = null,int? skip = null)
    {
        var albums = await _albumRepository
           .WhereSkipTakeAsync(album => album.Name.Contains(keyword),
           skip != null ? (int)skip : 0, take != null ? (int)take : 100);


        return albums.Select(a => new AlbumInfo(a));

    }

    public async Task<IEnumerable<ArtistInfo>> SearchArtist(string keyword, int? take = null, int? skip = null)
    {
      

        var artists = await _artistRepository
            .WhereSkipTakeAsync(artists => artists.Name.Contains(keyword),
            skip != null ? (int)skip : 0, take != null ? (int)take : 100);

        
        return artists.Select(a => new ArtistInfo(a));
    }
    public async Task<IEnumerable<PlaylistResult>> SearchPLaylist(string keyword,int? take = null, int? skip = null)
    {
        

        var playlists = await _playlistRepository.WhereSkipTakeAsync(p => p.Name.Contains(keyword), skip != null ? (int)skip : 0, take != null ? (int)take : 100);

        List<PlaylistResult> results = new List<PlaylistResult>();
        foreach (var playlist in playlists)
        {
            results.Add(new PlaylistResult(playlist));
        }
        return results;
    }

    public async Task<IEnumerable<GenreInfo>> SearchGenre(string keyword, int? take = null, int? skip = null)
    {
        var genre = await _genreRepository.WhereSkipTakeAsync(artists => artists.Name.Contains(keyword),
            skip != null ? (int)skip : 0, take != null ? (int)take : 100);


        return genre.Select(a => new GenreInfo(a));
    }
}
