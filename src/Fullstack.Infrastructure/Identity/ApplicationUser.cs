using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace Fullstack.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? Name { get; set; }
    }
}

