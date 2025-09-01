using Fullstack.Application.Interfaces;
using Fullstack.Domain.entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.Infrastructure.Services
{
    public class JwtSettings {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; } 
    }



    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;
        public JwtService(IOptions<JwtSettings> opts) { _settings = opts.Value; }
        public int AccessTokenExpirationMinutes => _settings.AccessTokenExpirationMinutes;
        public string GenerateAccessToken(User user, IList<string> roles)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
                };

            foreach (var r in roles) claims.Add(new Claim(ClaimTypes.Role, r));
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                _settings.Issuer,
                _settings.Audience,
                claims,
                now,
                now.AddMinutes(_settings.AccessTokenExpirationMinutes), creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            var random = new byte[64];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(random);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(random),
                Expires = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpirationDays),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }
    }
    }
