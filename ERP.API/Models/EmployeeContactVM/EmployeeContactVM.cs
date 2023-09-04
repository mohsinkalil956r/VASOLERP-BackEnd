using ERP.API.Models.EmployeeContactGetId;

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

        public EmployeeContactGetIdVM Contacts { get; set; }
    }
}
