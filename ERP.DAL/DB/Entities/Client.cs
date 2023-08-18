using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.DB.Entities
{
    public class Client : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Project> Projects { get; set; } = new();

    }
}
