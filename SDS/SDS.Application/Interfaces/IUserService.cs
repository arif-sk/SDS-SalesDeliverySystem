using SDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDS.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<AppUser>> GetAllUsers();

    }
}
