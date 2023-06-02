﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.DB.Entities
{
    public class SystemUser : IdentityUser<int>, IBaseEntity
    {
        public override int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; } = true;

        public List<Permission> Permissions { get; set; } = new();
        public List<Role> Roles { get; set; } = new();

        public List<UserRole> UserRoles { get; set; } = new();
        public List<UserPermission> UserPermissions { get; set; } = new();
    }
}
