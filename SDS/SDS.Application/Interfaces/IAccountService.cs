using SDS.Application.Models;
using SDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDS.Application.Interfaces
{
    public interface IAccountService
    {
        Task<AppUser> Login(string email, string password);
        Task<UserCredentialViewModel> GetUserCredential(int id);
        Task<AppUser> CreateUser(UserRegisterRequestViewModel userRegisterRequest, string password);
        Task<bool> UserExists(string email);
    }
}
