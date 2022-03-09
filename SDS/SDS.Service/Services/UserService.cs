using Caretaskr.Data;
using Caretaskr.Domain.Persistance;
using Microsoft.EntityFrameworkCore;
using SDS.Application.Interfaces;
using SDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDS.Domain.Enum.EnumExtensions;

namespace SDS.Service.Services
{
    public class UserService: ParentService, IUserService
    {
        private readonly ApplicationContext _applicationContext;

        public UserService(IGenericUnitOfWork genericUnitOfWork, ApplicationContext applicationContext) : 
            base (genericUnitOfWork)
        {
            _applicationContext = applicationContext;
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await (from ap in _applicationContext.Users
                          join ud in _applicationContext.UserDetails on ap.UserDetail.Id equals ud.Id
                          join ur in _applicationContext.UserRoles on ap.Id equals ur.AppUser.Id
                          join r in _applicationContext.Roles on ur.Role.Id equals r.Id
                          select new AppUser
                          {
                              Id = ap.Id,
                              Email = ap.Email,
                              UserDetail = new UserDetail{ 
                                 FirstName = ud.FirstName,
                                 LastName = ud.LastName,
                                 Address = ud.Address,
                                 DOB = ud.DOB,
                                 Id = ud.Id
                              },
                              Phone = ap.Phone,
                              UserRole = r.Name
                          }).ToListAsync();
        }
    }
}
