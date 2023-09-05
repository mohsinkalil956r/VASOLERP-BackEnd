using ERP.API.Models.EmployeeContactGetId;
using ERP.DAL.DB.Entities;
namespace ERP.API.Models.DepartmentController
{
    public class DepartmentGetByIdVM
    {
        public int Id { get; set; }
        public string Name{ get; set; }
        public string Hod { get; set; }
    }
}
