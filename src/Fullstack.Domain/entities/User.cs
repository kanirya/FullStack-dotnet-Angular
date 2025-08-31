using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.Domain.entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Role { get; private set; }
        private User() { }
        public User(Guid id, string name, string email, string role = "User")
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Name = name;
            Email = email;
            Role = string.IsNullOrWhiteSpace(role) ? "User" : role;
        }
    }
}
