using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDS.Application.Models
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class UserLoginViewModel
    {
        public string Email { get; set;}
        public string Password { get; set; }
    }

    public class UserRegisterRequestViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }

    public class UserCredentialViewModel
    {
        public int UserId { get; set; }
        public string Email { get; set;}
        public string Role { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }

        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
