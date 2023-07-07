using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Microsoft.ML.Data;
using MusicApp.Algorithm.Common;
using MusicApp.Application.Common.Interface.Algorithm;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Domain.Common.Entities;

namespace MusicApp.Algorithm.Persistence;

internal class SongClustering : IClustering
{
    private readonly MLContext _mlContext;
    private readonly string _connectionString;
    private readonly IFileRepository _fileRepository;



    public SongClustering(IOptions<DatabaseSourceSetting> options, IFileRepository fileRepository)
    {

        _mlContext = new MLContext();
        _connectionString = options.Value.ConnectionString;
        _fileRepository = fileRepository;
    }

    public async Task<IEnumerable<string>> GetClusters(params string[] ids)
    {
        var loader = _mlContext.Data.CreateDatabaseLoader<SongTrainData>();
        string sqlCommand = "SELECT * From SongData";
        DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, _connectionString, sqlCommand);
        var data = loader.Load(dbSource);
        var model = await LoadModel("model.zip");
        var songs = _mlContext.Data
          .CreateEnumerable<SongTrainData>(data, reuseRowObject: false)
          .Where(s => ids.Any(i => i == s.id));
        var songMean = new SongTrainData()
        {
            id = " ",
            danceability = songs.Average(song => song.danceability),
            acousticness = songs.Average(song => song.acousticness),
            duration = songs.Average(song => song.duration),
            energy = songs.Average(song => song.energy),
            instrumentalness = songs.Average(song => song.instrumentalness),
            key = songs.Average(song => song.key),
            liveness = songs.Average(song => song.liveness),
            loudness = songs.Average(song => song.loudness),
            mode = songs.Average(song => song.mode),
            speechiness = songs.Average(song => song.speechiness),
            tempo = songs.Average(song => song.tempo),
            time_signature = songs.Average(song => song.time_signature),
            valence = songs.Average(song => song.valence),
        };
        var songCenter = _mlContext.Data.LoadFromEnumerable(new List<SongTrainData>() { songMean });


        var scaledData = model.Transform(data);
        var scaledSong = model.Transform(songCenter);


        var songList = _mlContext.Data.
            CreateEnumerable<ClusterPrediction>(scaledSong, reuseRowObject: false).First();

        var dataList = _mlContext.Data
            .CreateEnumerable<ClusterPrediction>(scaledData, reuseRowObject: false);


        var results = dataList.Where(a => a.PredictedClusterId == songList.PredictedClusterId)
            .OrderBy(r => SquareDistance(r.Distances, songList.Distances)).Take(20);


        return results.Select(r => r.Id);
    }
    public async Task<ITransformer> LoadModel(string fileName)
    {
        using (var ms = new MemoryStream())
        {
            await _fileRepository.DownloadAsync(FileType.Model, ms, fileName);
            DataViewSchema columns;
            return _mlContext.Model.Load(ms, out columns);
        }

    }
    public async Task<ModelResult> TrainModel(int clusterNumber)
    {

        var loader = _mlContext.Data.CreateDatabaseLoader<SongTrainData>();
        string sqlCommand = "SELECT * From SongData";
        DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, _connectionString, sqlCommand);
        var data = loader.Load(dbSource);
        string[] numberCols = new string[] { "acousticness","danceability", "duration", "energy",
                "instrumentalness", "key", "liveness", "loudness", "mode",
                "speechiness", "tempo", "time_signature", "valence" };

        var pipeLine = _mlContext.Transforms.Concatenate("res", numberCols)
            .Append(_mlContext.Transforms.NormalizeMinMax("resNormal", "res"))
            .Append(_mlContext.Clustering.Trainers.KMeans("resNormal", numberOfClusters: clusterNumber));


        var model = pipeLine.Fit(data);
        using (var ms = new MemoryStream())
        {
            _mlContext.Model.Save(model, null, ms);
            await _fileRepository.UploadAsync(FileType.Model, ms, "model.zip");
        }
        var predictions = model.Transform(data);
        var metrics = _mlContext.Clustering.Evaluate(predictions, scoreColumnName: "Score", featureColumnName: "resNormal");
        var clusterResult = _mlContext.Data.CreateEnumerable<ClusterResult>(predictions, reuseRowObject: false);

        return new ModelResult()
        {
            Metrics = metrics,
            ClusterResult = clusterResult
        };
     

    }
    double SquareDistance(float[]? vec1, float[]? vec2)
    {
        if (vec1 is null || vec2 is null) return float.MaxValue;
        double sqrDis = 0;
        for (int i = 0; i < vec1.Length; i++)
        {
            sqrDis += Math.Pow(vec1[i] - vec2[i], 2);
        }
        return sqrDis;
    }
}
