using Fullstack.Application.DTOs;
using Fullstack.Domain.entities;

using Fullstack.Domain.Interfaces;
using Fullstack.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Fullstack.Infrastructure.Repositories
{
    public class UserDataRepository : IUserDataRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserDataRepository(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserDataDtos> GetByIdAsync(Guid id)
        {
            // Get logged-in user from JWT (sub claim)
            var sub = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (sub == null) return null;

            // The id from JWT (sub) will be a string (GUID)
            if (sub == id.ToString())
            {
                // ✅ Same user – return user with extra "Admin" property
                var user = await _userManager.FindByIdAsync(sub);

                if (user == null) return null;

                return new  UserDataDtos(
                   user.Id.ToString(),
                    user.Email,
                    user.Name,
                    true
                );
            }
            else
            {
                // ✅ Different user – fetch by id and return without admin
                var user = await _userManager.FindByIdAsync(id.ToString());

                if (user == null) return null;

                return new UserDataDtos(
                   user.Id.ToString(),
                    user.Email,
                    user.Name,
                    false
                );
            }
        }
    }
}
