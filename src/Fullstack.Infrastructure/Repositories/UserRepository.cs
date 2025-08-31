using Fullstack.Application.Interfaces;
using Fullstack.Domain.entities;
using Fullstack.Infrastructure.Data;
using Fullstack.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Fullstack.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AppDbContext _db;
        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, AppDbContext db)
        {
            _userManager = userManager; _roleManager = roleManager; _db = db;
        }
        public async Task<(bool Succeeded, string[] Errors)> CreateAsync(User user, string password)
        {
            var appUser = new ApplicationUser { Id = user.Id, UserName = user.Email, Email = user.Email, Name = user.Name };
            var res = await _userManager.CreateAsync(appUser, password);
            if (!res.Succeeded) return (false, res.Errors.Select(e => e.Description).ToArray());
            var role = user.Role ?? "User";
            if (!await _roleManager.RoleExistsAsync(role)) await _roleManager.CreateAsync(new ApplicationRole { Name = role });
            await _userManager.AddToRoleAsync(appUser, role);
            return (true, Array.Empty<string>());
        }
        public async Task<User?> FindByEmailAsync(string email)
        {
            var app = await _userManager.FindByEmailAsync(email);
            return app == null ? null : MapToDomain(app);
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            var app = await _userManager.FindByIdAsync(id.ToString());
            return app == null ? null : MapToDomain(app);
        }
        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var app = await _userManager.FindByIdAsync(user.Id.ToString());
            if (app == null) return false;
            return await _userManager.CheckPasswordAsync(app, password);
        }
        public async Task<IList<string>> GetRolesAsync(User user)
        {
            var app = await _userManager.FindByIdAsync(user.Id.ToString());
            if (app == null) return new List<string>();
            return await _userManager.GetRolesAsync(app);
        }
        public async Task AddToRoleAsync(User user, string role)
        {
            var app = await _userManager.FindByIdAsync(user.Id.ToString());
            if (app == null) throw new Exception("User not found");
            if (!await _roleManager.RoleExistsAsync(role)) await _roleManager.CreateAsync(new ApplicationRole { Name = role });
            await _userManager.AddToRoleAsync(app, role);
        }
        public async Task EnsureRoleExistsAsync(string role)
        {
            if (!await _roleManager.RoleExistsAsync(role)) await _roleManager.CreateAsync(new ApplicationRole { Name = role });
        }
        private User MapToDomain(ApplicationUser app) => new User(app.Id, app.Name ?? string.Empty, app.Email ?? string.Empty, GetPrimaryRole(app).Result ?? "User");
        private async Task<string?> GetPrimaryRole(ApplicationUser app)
        {
            var roles = await _userManager.GetRolesAsync(app);
            return roles.FirstOrDefault();
        }
    }
}