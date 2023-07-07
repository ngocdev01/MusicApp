using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.Common.Entities;

public class ClusterPrediction
{
    [ColumnName("id")]
    public string Id = null!;

    [ColumnName("PredictedLabel")]
    public uint PredictedClusterId;

    [ColumnName("Score")]
    public float[]? Distances;
}

public class ClusterResult
{
    [ColumnName("id")]
    public string Id = null!;

    [ColumnName("PredictedLabel")]
    public uint PredictedClusterId;

    [ColumnName("resNormal")]
    public float[]? Distances;
}

public class ModelResult
{
    public IEnumerable<ClusterResult> ClusterResult { get; set; } = null!;
    public ClusteringMetrics Metrics { get; set; } = null!;
}