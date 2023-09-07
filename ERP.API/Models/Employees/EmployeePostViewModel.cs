using ERP.API.Models.Contacts;

namespace ERP.API.Models.Employees
{
    public class EmployeePostViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string CNIC { get; set; }
        public double Salary { get; set; }

        public DateTime ContractDate { get; set; }
        public int DepartmentId { get; set; }
        public ContactPostViewModel Contact { get; set; }
    }
}
