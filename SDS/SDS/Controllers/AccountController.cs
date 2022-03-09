using Caretaskr.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SDS.Application.Interfaces;
using SDS.Application.Models;
using SDS.Data;
using SDS.Domain.Entities;
using SDS.Token.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SDS.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ApplicationContext _context;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _conf;
        private readonly IAccountService _accountService;

        public AccountController(ApplicationContext context, ITokenService tokenService,
            IConfiguration conf, IAccountService accountService)
        {
            _context = context;
            _tokenService = tokenService;
            _conf = conf;
            _accountService = accountService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserViewModel>> RegisterUser(UserRegisterRequestViewModel register)
        {
            register.Email = register.Email.ToLower();
            if (!await _accountService.UserExists(register.Email)) return BadRequest("Username is taken");
            var user = await _accountService.CreateUser(register, register.Password);
            return new UserViewModel
            {
                Email = user.Email
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserViewModel>> Login(UserLoginViewModel login)
        {
            var user = await _accountService.Login(login.Email, login.Password);
            if (user == null) return Unauthorized("Invalid User");
            UserCredentialViewModel userCredentialView = await _accountService.GetUserCredential(user.Id);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userCredentialView.UserId.ToString()),
                new Claim(ClaimTypes.Email,userCredentialView.Email),
                new Claim(ClaimTypes.Role, userCredentialView.Role),
                new Claim(ClaimTypes.Name, userCredentialView.FullName),
                new Claim(ClaimTypes.SerialNumber, userCredentialView.UserId.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8
                         .GetBytes(_conf.GetSection("TokenKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
        [Authorize()]
        [HttpGet("allusers")]
        public async Task<IActionResult> GetUser()
        {
            return Ok();
        }
    }
}
