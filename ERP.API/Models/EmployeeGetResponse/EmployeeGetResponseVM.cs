using ERP.API.Models.DepartmentController;

namespace ERP.API.Models.EmployeeGetResponse
{
    public class EmployeeGetResponseVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string CNIC { get; set; }
        public double Salary { get; set; }

        public DateTime ContractDate { get; set; }

        public DepartmentGetVM Department { get; set; }
        public List<EmployeeContacts.EmployeeContactPutVM> Contacts { get; set; }

    }
}
