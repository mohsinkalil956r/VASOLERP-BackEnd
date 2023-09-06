using ERP.API.Models.DepartmentController;
using ERP.API.Models.EmployeeContactGetId;
using ERP.DAL.DB.Entities;

namespace ERP.API.Models.EmployeeContactVM
{
    public class EmployeeContactVM
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string CNIC { get; set; }
        public double Salary { get; set; }
        public DateTime ContractDate { get; set; }
        public int DepartmentId { get; set; }
        public EmployeeContactGetIdVM Contact { get; set; }
        //public DepartmentGetVM Department { get; set; }
        //public DepartmentGetByIdVM Department { get; set; }
    }
}
