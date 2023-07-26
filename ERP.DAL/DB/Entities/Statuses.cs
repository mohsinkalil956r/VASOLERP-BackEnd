using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.DB.Entities
{
    public class Statuses : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string Name { get; set; }
        public bool IsProgress { get; set; }

        public int Progress { get;set; }

        public List<Project> Projects { get; set; } = new();

    }
}
