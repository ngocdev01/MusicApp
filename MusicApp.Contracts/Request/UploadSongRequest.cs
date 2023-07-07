using Microsoft.AspNetCore.Http;


namespace MusicApp.Contracts.Request;

public record CreateSongRequest
(
    string songName,
    string[] artists,
    string album,
    string[]? genres,
    string songAudio   
);
