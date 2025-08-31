using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.Domain.entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; } = string.Empty;
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; } = string.Empty;
        public string ReplacedByToken { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public bool IsActive => !IsRevoked && DateTime.UtcNow <= Expires;
    }
}
