using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.DB.Entities
{
    public class ProjectEmployee : IBaseEntity
    {
        [NotMapped]
        public int Id { get; set; }

        [NotMapped]
        public bool IsActive { get; set; } = true;

        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public Project Project { get; set; }

    }
}
