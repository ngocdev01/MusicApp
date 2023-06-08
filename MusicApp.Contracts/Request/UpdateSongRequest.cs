using Microsoft.AspNetCore.Http;


namespace MusicApp.Contracts.Request;

public record UpdateSongRequest
(
    string songName,
    string[] artists,
    string album,
    string[]? genres,
    IFormFile? songAudio   
);
