﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.DB.Entities
{
    public class Department : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string Name { get; set; }

        public string Dob { get;set; }

        public List<Employee> Employees { get; set; } = new();

    }
}
