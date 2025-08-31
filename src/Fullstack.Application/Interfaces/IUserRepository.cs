using Fullstack.Domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<(bool Succeeded, string[] Errors)> CreateAsync(User user, string password);
        Task<User?> FindByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IList<string>> GetRolesAsync(User user);
        Task AddToRoleAsync(User user, string role);
        Task EnsureRoleExistsAsync(string role);
    }
}
