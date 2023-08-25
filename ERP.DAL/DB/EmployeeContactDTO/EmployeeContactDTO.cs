using ERP.DAL.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.DB.EmployeeContactDTO
{
    public class EmployeeContactDTO
    {
        public Employee Employee { get; set; }
        public Contact Contact { get; set; }
    
    }
}
