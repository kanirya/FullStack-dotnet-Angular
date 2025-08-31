using Fullstack.Application.Interfaces;
using Fullstack.Domain.entities;
using Fullstack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _db;
        public RefreshTokenRepository(AppDbContext db) { _db = db; }
        public async Task AddAsync(RefreshToken token) { await _db.RefreshTokens.AddAsync(token); }
        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _db.RefreshTokens.SingleOrDefaultAsync(r => r.Token == token);
        }
        public async Task SaveChangesAsync() { await _db.SaveChangesAsync(); }
    }
}
