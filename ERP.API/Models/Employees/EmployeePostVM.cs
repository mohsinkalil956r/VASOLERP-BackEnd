﻿namespace ERP.API.Models.Employees
{
    public class EmployeePostVM
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string CNIC { get; set; }
    }
}
