using Caretaskr.Data;
using Caretaskr.Domain.Persistance;
using Microsoft.EntityFrameworkCore;
using SDS.Application.Interfaces;
using SDS.Application.Models;
using SDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SDS.Service.Services
{
    public class AccountService : ParentService, IAccountService
    {
        private readonly ApplicationContext _applicationContext;
        public AccountService(IGenericUnitOfWork genericUnitOfWork, ApplicationContext applicationContext) :
            base(genericUnitOfWork)
        {
            _applicationContext = applicationContext;
        }

        public async Task<AppUser> CreateUser(UserRegisterRequestViewModel register, string password)
        {
            var userRepo = _genericUnitOfWork.GetRepository<AppUser>();
            var roleRepo = _genericUnitOfWork.GetRepository<Role>();
            var userRoleRepo = _genericUnitOfWork.GetRepository<UserRole>();

            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                Email = register.Email.ToLower(),
                Phone = register.Phone,
                UserDetail = new UserDetail()
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Address = register.Address,
                    DOB = register.DOB
                },
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
                PasswordSalt = hmac.Key
            };
            userRepo.Add(user);
            var role = await roleRepo.FirstOrDefault(x => x.Name == register.Role);
            var userRole = new UserRole
            {
                AppUser = user,
                Role = role
            };
            userRoleRepo.Add(userRole);
            await _genericUnitOfWork.SaveChangesAsync();
            return user;
        }

        public async Task<UserCredentialViewModel> GetUserCredential(int id)
        {
            return await (from ap in _applicationContext.Users
                          join ud in _applicationContext.UserDetails on ap.UserDetail.Id equals ud.Id
                          join ur in _applicationContext.UserRoles on ap.Id equals ur.AppUser.Id
                          join r in _applicationContext.Roles on ur.Role.Id equals r.Id
                          where (ap.Id == id)
                          select new UserCredentialViewModel
                          {
                              UserId = ap.Id,
                              Email = ap.Email,
                              FirstName = ap.UserDetail.FirstName,
                              LastName = ap.UserDetail.LastName,
                              Role = r.Name,
                              FullName = ap.UserDetail.FirstName + " " + ap.UserDetail.LastName
                          }).FirstOrDefaultAsync();

        }

        public async Task<AppUser> Login(string email, string password)
        {
            var userRepo = _genericUnitOfWork.GetRepository<AppUser>();
            var user = await userRepo.FirstOrDefault(x => x.Email == email);
            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            return user;
        }

        public async Task<bool> UserExists(string email)
        {
            var userRepo = _genericUnitOfWork.GetRepository<AppUser>();
            var user = await userRepo.FirstOrDefault(x => x.Email == email);
            if (user != null)
                return false;
            return true;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
            }
            return true;
        }
    }
}
