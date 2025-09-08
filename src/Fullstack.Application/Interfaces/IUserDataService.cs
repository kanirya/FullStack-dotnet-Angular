using Fullstack.Application.DTOs;
using Fullstack.Domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.Application.Interfaces
{
    public interface IUserDataService
    {
        Task<UserDataDtos?>  GetByIdAsync(Guid id);
    }
}
