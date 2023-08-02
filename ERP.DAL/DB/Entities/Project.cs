using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.DB.Entities
{
    public class Project : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeadLine { get; set; }
        public double Budget { get; set; }

        public int ClientId { get; set; }
      
        public int StatusId { get; set; }

        public Client Client { get; set; }
        public Statuses Status { get; set; }

        public List<Client> Clients { get; set; } = new();


        public List<ProjectEmployee> ProjectEmployees { get; set; } = new();

        public List<Employee> Employees { get; set; } = new();
    }
}
