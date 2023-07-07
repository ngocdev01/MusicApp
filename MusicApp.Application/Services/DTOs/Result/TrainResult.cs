
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Application.Services.DTOs.Result;

public record ClusterTrainResult(string Id,uint PredictedClusterId,float[]? Distances) { }
public record ModelTrainResult(List<ClusterTrainResult> Clusters,double AverageDistance,double DaviesBouldinIndex);