
using MusicApp.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Common.Interface.Algorithm;

public interface IClustering
{
    Task<IEnumerable<string>> GetClusters(params string[] ids);
    Task<ModelResult> TrainModel(int clusterNumber);
}
