using ERP.API.Models.EmployeeContactGetId;

namespace ERP.API.Models.Employees
{
    public class EmployeePutVM :EmployeePostVM
    {
        public EmployeeContactGetIdVM Contact { get; set; }
    }
}
