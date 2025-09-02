using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.Application.DTOs
{
    public record RegisterDto(string Name, string Email, string Password, string Role);
    public record LoginDto(string Email, string Password);
    public record TokenDto(string AccessToken, string RefreshToken, DateTime ExpiresAt);
    public record RefreshRequestDto(string RefreshToken);
    public record ReturnDataDto(string AccessToken, string RefreshToken, DateTime ExpiresAt, UserDto ReturnUserData);
    public record UserDto(string Name,Guid Uid, string Email,string Role,DateTime loginDate);
}
