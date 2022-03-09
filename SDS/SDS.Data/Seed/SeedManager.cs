using Microsoft.EntityFrameworkCore;
using SDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SDS.Data.Seed
{
    public class SeedManager
    {
        protected ModelBuilder MigrationBuilder { get; }

        protected static DateTime InitialRecreateDate = new DateTime(2022, 1, 1);
        public SeedManager(ModelBuilder migrationBuilder)
        {
            MigrationBuilder = migrationBuilder;
            this.Initialize();
        }
        private void Initialize()
        {
        }

        public void Seed()
        {
            this.SeedUserDetail();
            this.SeedUsers();
            this.SeedRole();
            this.SeedUserRole();
        }
        protected void SeedUsers()
        {
            var now = InitialRecreateDate;
            using var hmac = new HMACSHA512();

            MigrationBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = 1,
                Email = "admin@gmail.com",
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Admin@123")),
                PasswordSalt = hmac.Key,
                Phone = "01711230955",
                UserDetailId = 1
            });
        }
        protected void SeedUserDetail()
        {
            MigrationBuilder.Entity<UserDetail>().HasData(new UserDetail
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "User",
                Address = "Dhaka",
                DOB = InitialRecreateDate
            });
        }
        protected void SeedRole()
        {
            MigrationBuilder.Entity<Role>().HasData(new Role
            {
                Id = 1,
                Name = "Admin",
                IsDisabled = false,
                Level = 0
            });
            MigrationBuilder.Entity<Role>().HasData(new Role
            {
                Id = 2,
                Name = "OrderGeneration",
                IsDisabled = false,
                Level = 1
            });
            MigrationBuilder.Entity<Role>().HasData(new Role
            {
                Id = 3,
                Name = "OrderApproval",
                IsDisabled = false,
                Level = 2
            });
            MigrationBuilder.Entity<Role>().HasData(new Role
            {
                Id = 4,
                Name = "Manufacture",
                IsDisabled = false,
                Level = 3
            });
            MigrationBuilder.Entity<Role>().HasData(new Role
            {
                Id = 5,
                Name = "QACheck",
                IsDisabled = false,
                Level = 4
            });
            MigrationBuilder.Entity<Role>().HasData(new Role
            {
                Id = 6,
                Name = "FinalCheck",
                IsDisabled = false,
                Level = 5
            });
            MigrationBuilder.Entity<Role>().HasData(new Role
            {
                Id = 7,
                Name = "CustomerCare",
                IsDisabled = false,
                Level = 6
            });
        }

        protected void SeedUserRole()
        {
            MigrationBuilder.Entity<UserRole>().HasData(new UserRole
            {
                Id = 1,
                AppUserId = 1,
                RoleId = 1
            });

        }
    }
}
