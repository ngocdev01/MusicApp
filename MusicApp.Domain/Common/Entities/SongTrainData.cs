using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.Common.Entities;

public class SongTrainData
{
    public string id { get; set; } = null!;
    public float acousticness { get; set; }
    public float danceability { get; set; }
    public float duration { get; set; }
    public float energy { get; set; }
    public float instrumentalness { get; set; }
    public float key { get; set; }
    public float liveness { get; set; }
    public float loudness { get; set; }
    public float mode { get; set; }
    public float speechiness { get; set; }
    public float tempo { get; set; }
    public float time_signature { get; set; }
    public float valence { get; set; }
}
