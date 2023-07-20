using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.DB.Entities
{
    public class Employee : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string CNIC { get; set; }

        public double Salary { get; set; }
        public List<Project> Projects { get; set; } = new();
        public List<ProjectEmployee> ProjectEmployees { get; set; } = new();
        public List<AssetIssuance> AssetIssuances { get; set; } = new();
        public List<Asset> Assets { get; set; } = new();
        public List<EmployeeContact> EmployeeContacts { get; set; } = new();
       
    }
}
