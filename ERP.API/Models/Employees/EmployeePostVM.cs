namespace ERP.API.Models.Employees
{
    public class EmployeePostVM
    {
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string CNIC { get; set; }
        public double Salary { get; set; }

        public List<EmployeeContacts.EmployeeContactPutVM> Contacts { get; set; }

    }
}
