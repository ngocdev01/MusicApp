using MusicApp.Domain.Common.Entities;

namespace MusicApp.Application.Services.DTOs.ObjectInfo;

public record UserInfo(string? Id,string UserName,string Email,string Password,string RoleId) { }

