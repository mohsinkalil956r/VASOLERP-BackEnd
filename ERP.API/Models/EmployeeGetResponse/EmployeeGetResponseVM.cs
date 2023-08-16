using ERP.API.Models.DepartmentController;
using ERP.API.Models.EmployeeContactGetResponse;

namespace ERP.API.Models.EmployeeGetResponse
{
    public class EmployeeGetResponseVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string CNIC { get; set; }
        public double Salary { get; set; }

        public DateTime ContractDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }

        public DepartmentGetVM Department { get; set; }
        //public List<EmployeeContactGetResponseVM> Contacts { get; set; }

    }
}
