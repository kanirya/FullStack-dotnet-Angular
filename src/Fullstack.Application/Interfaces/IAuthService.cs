using Fullstack.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ReturnDataDto> RegisterAsync(RegisterDto dto, string ip);
        Task<ReturnDataDto> LoginAsync(LoginDto dto, string ip);
        Task<TokenDto> RefreshAsync(string refreshToken, string ip);
        Task RevokeAsync(string refreshToken, string ip);
    }
}
