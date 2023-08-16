namespace ERP.API.Models.Employees
{
    public class EmployeePostVM
    {
       
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

        public int DepartmentId { get; set; }

    }
}
