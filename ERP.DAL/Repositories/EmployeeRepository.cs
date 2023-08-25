using ERP.DAL.DB.Entities;
using ERP.DAL.DB;
using ERP.DAL.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.DAL.DB.EmployeeContactDTO;

namespace ERP.DAL.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        private readonly ERPContext _context;
        public EmployeeRepository(ERPContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<EmployeeContactDTO> GetEmployeeWithContact()
        {
            var query = from c in this._context.Contacts
                         where c.ReferenceId.HasValue && c.Type == "Employee"
                         join e in this._context.Employees on c.ReferenceId.Value equals e.Id
                         select new EmployeeContactDTO
                         {
                             Employee = e,
                             Contact = c
                         };

            return query; 

        }


    }
}

