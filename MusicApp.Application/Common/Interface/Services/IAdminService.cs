using MusicApp.Application.Services.DTOs.ObjectInfo;
using MusicApp.Application.Services.DTOs.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Common.Interface.Services;

public interface IAdminService
{
    Task<IEnumerable<KeyValuePair<DateTime,int>>> GetPlayTimeChart(DateTime from, DateTime to);
    Task<IEnumerable<KeyValuePair<DateTime, int>>> GetPlayTimeChart(DateTime month);
    Task<IEnumerable<RoleInfo>> Roles();
    AppInfomationResult GetAppInfomationResult();
    Task<ModelTrainResult> TrainModel(int clusterNumber);
}
