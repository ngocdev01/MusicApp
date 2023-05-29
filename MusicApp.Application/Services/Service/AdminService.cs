using Microsoft.EntityFrameworkCore;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Application.Common.Interface.Services;
using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MusicApp.Application.Services.Service;

public class AdminService : IAdminService
{
    private readonly IRepository<Song> _songRepositoy;
    private readonly IRepository<Artist> _artistRepositoy;
    private readonly IRepository<Album> _albumRepositoy;
    private readonly IRepository<Playlist> _playlistRepositoy;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IRepository<UserSongEvent> _songEventRepository;

    public AdminService(IRepository<Song> songRepositoy,
                        IRepository<Artist> artistRepositoy,
                        IRepository<Album> albumRepositoy,
                        IRepository<Playlist> playlistRepositoy,
                        IRepository<User> userRepository,
                        IRepository<UserSongEvent> songEventRepository,
                        IRepository<Role> roleRepository)
    {
        _songRepositoy = songRepositoy;
        _artistRepositoy = artistRepositoy;
        _albumRepositoy = albumRepositoy;
        _playlistRepositoy = playlistRepositoy;
        _userRepository = userRepository;
        _songEventRepository = songEventRepository;
        _roleRepository = roleRepository;
    }


    public async Task<IEnumerable<KeyValuePair<DateTime, int>>> GetPlayTimeChart(DateTime month)
    {
       
        var data = new List<KeyValuePair<DateTime, int>>();
        var days = DateTime.DaysInMonth(month.Year,month.Month);
        
        var start = new DateTime(month.Year, month.Month, 1);
        for (var i = 0; i < days; i ++)
        {
            data.Add(new(start.AddDays(i).Date, 0));
        }
        var chart = _songEventRepository.GetQuery()
           .Where(e => e.Time.Month == month.Month && e.Time.Year == month.Year)
           .GroupBy(e => e.Time.Date)
           .Select(e => new KeyValuePair<DateTime, int>(e.Key.Date, e.Count()));
        var res = await _songEventRepository.GetListAsync(chart);

        for (int i = 0, j = 0; i < days && j < data.Count(); i++)
        {
            var day = data[i].Key;
            var result = res.ElementAtOrDefault(j).Key;
            if (day.Day == result.Day)
            {
                data[i] = res.ElementAtOrDefault(j);
                j++;
            }
        }

        return data;
    }
    public async Task<IEnumerable<KeyValuePair<DateTime, int>>> GetPlayTimeChart(DateTime from, DateTime to)
    {
        var data = new List<KeyValuePair<DateTime,int>>();

        for (var dt = from; dt <= to; dt = dt.AddMonths(1))
        {
            data.Add(new (dt,0));
        }

        var chart = _songEventRepository.GetQuery()
            .Where(e => e.Time >= from && e.Time <= to)
            .GroupBy(e => new {Month  = e.Time.Month, Year = e.Time.Year})
            .Select(e => new KeyValuePair<DateTime, int>(new DateTime(e.Key.Year,e.Key.Month,1), e.Count()));
        var res = await _songEventRepository.GetListAsync(chart);

        for (int i = 0, j = 0; i < data.Count && j < res.Count(); i++)
        {
            var month = data[i].Key;
            var result = res.ElementAtOrDefault(j).Key;
            if (month.Month == result.Month && month.Year == result.Year)
            {
                data[i] = res.ElementAtOrDefault(j);
                j++;
            }
        }

        return data;
                         
    }

    public async Task<IEnumerable<RoleInfo>> Roles()
    {
        return (await _roleRepository.GetAll()).Select(r => new RoleInfo(r));
    }
}
