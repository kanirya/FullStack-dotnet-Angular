using Fullstack.Domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user, IList<string> roles);
        RefreshToken GenerateRefreshToken(string ipAddress);
        int AccessTokenExpirationMinutes { get; }
    }
}
