using Caretaskr.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDS.Application.Interfaces;
using SDS.Data;
using SDS.Domain.Entities;

namespace SDS.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly ApplicationContext _context;
        private readonly IUserService _userService;
        public UsersController(ApplicationContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users =  await _userService.GetAllUsers();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("id")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }

    }
}
