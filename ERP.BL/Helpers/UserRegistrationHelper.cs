﻿using Microsoft.AspNetCore.Identity;
using ERP.DAL.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.BL.Helpers
{
    public class UserRegistrationHelper
    {
        public static SystemUser GetUserEntity(string username, string firstName, string lastName, string phoneNumber, string password)
        {
            var userEntity = new SystemUser()
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = username,
                NormalizedUserName = username.ToUpper(),
                Email = username,
                NormalizedEmail = username.ToUpper(),
                LockoutEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = phoneNumber,
                IsActive = true,
                PhoneNumberConfirmed = true,
            };

            var passwordHasher = new PasswordHasher<SystemUser>();
            userEntity.PasswordHash = passwordHasher.HashPassword(userEntity, password);

            return userEntity;
        }

        public static string GenerateTemporaryPassword()
        {
            return "Admin*123";
        }
    }
}
