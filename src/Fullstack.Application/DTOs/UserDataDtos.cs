using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.Application.DTOs
{
    public record UserDataDtos(string Uid, string Name, string Email,bool isAdmin );
}
