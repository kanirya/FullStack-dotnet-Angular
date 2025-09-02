using Fullstack.Application.DTOs;
using Fullstack.Application.Interfaces;
using Fullstack.Domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly IJwtService _jwt;
        public AuthService(IUserRepository userRepo, IRefreshTokenRepository refreshRepo, IJwtService jwt)
        {
            _userRepo = userRepo;
            _refreshRepo = refreshRepo; 
            _jwt = jwt;
        }
        public async Task<ReturnDataDto> RegisterAsync(RegisterDto dto, string ip)
        {
            var role = string.IsNullOrWhiteSpace(dto.Role) ? "User" : dto.Role;
            await _userRepo.EnsureRoleExistsAsync(role);
            var domainUser = new User(Guid.NewGuid(), dto.Name, dto.Email, role);
            var (succeeded, errors) = await _userRepo.CreateAsync(domainUser, dto.Password);
            if (!succeeded) throw new Exception(string.Join(';', errors));
            var roles = await _userRepo.GetRolesAsync(domainUser);
            var access = _jwt.GenerateAccessToken(domainUser, roles);
            var refresh = _jwt.GenerateRefreshToken(ip);
            refresh.UserId = domainUser.Id;
            await _refreshRepo.AddAsync(refresh);
            await _refreshRepo.SaveChangesAsync();
            var userData = new UserDto(domainUser.Name, domainUser.Id, domainUser.Email, domainUser.Role, DateTime.UtcNow);
            return new ReturnDataDto(access, refresh.Token, DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpirationMinutes),userData);
        }

        public async Task<ReturnDataDto> LoginAsync(LoginDto dto, string ip)
        {
            var user = await _userRepo.FindByEmailAsync(dto.Email) ?? throw new Exception("Invalid Email");
            if (!await _userRepo.CheckPasswordAsync(user, dto.Password)) throw new Exception("Invalid Password");
            var roles = await _userRepo.GetRolesAsync(user);
            var access = _jwt.GenerateAccessToken(user, roles);
            var refresh = _jwt.GenerateRefreshToken(ip);
            refresh.UserId = user.Id;
            await _refreshRepo.AddAsync(refresh);
            await _refreshRepo.SaveChangesAsync();
            var userData = new UserDto(user.Name,user.Id, user.Email, user.Role,DateTime.UtcNow);
            return new ReturnDataDto(access, refresh.Token, DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpirationMinutes),userData );
        }



        public async Task<TokenDto> RefreshAsync(string refreshToken, string ip)
        {
            var rt = await _refreshRepo.GetByTokenAsync(refreshToken) ?? throw new Exception("Invalid refresh token");
            if (!rt.IsActive) throw new Exception("Invalid refresh token");
            rt.IsRevoked = true; 
            rt.Revoked = DateTime.UtcNow;
            rt.RevokedByIp = ip;


            var newRt = _jwt.GenerateRefreshToken(ip);
            newRt.UserId = rt.UserId;
            rt.ReplacedByToken = newRt.Token;


            await _refreshRepo.AddAsync(newRt); 
            await _refreshRepo.SaveChangesAsync();

            var user = await _userRepo.GetByIdAsync(rt.UserId) ?? throw new Exception("User not found");
            var roles = await _userRepo.GetRolesAsync(user);
            var access = _jwt.GenerateAccessToken(user, roles);
          
            return new TokenDto(access, newRt.Token, DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpirationMinutes));
        }
        public async Task RevokeAsync(string refreshToken, string ip)
        {
            var rt = await _refreshRepo.GetByTokenAsync(refreshToken);
            if (rt == null || !rt.IsActive) return;
            rt.IsRevoked = true;
            rt.Revoked = DateTime.UtcNow;
            rt.RevokedByIp = ip;
            await _refreshRepo.SaveChangesAsync();
        }




    }
}
