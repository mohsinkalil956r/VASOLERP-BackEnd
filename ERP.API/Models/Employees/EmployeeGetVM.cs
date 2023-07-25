using ERP.DAL.DB.Entities;

namespace ERP.API.Models.Employees
{
    public class EmployeeGetVM : EmployeePostVM
    {
        public virtual ICollection<EmployeeContact> EmployeeContacts { get; set; }

    }
}
