using ERP.API.Models.ClientGetResponse;
using ERP.API.Models.StatusGetResponse;

namespace ERP.API.Models.ProjectGetResponse
{
    public class ProjectGetResponseVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PlannedCompletedAt { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public double Budget { get; set; }
        public string Location { get; set; }
        
        public ProjectClientVM Client { get; set; }

        public ProjectStatusVM Status { get; set; }
        public List<int> EmployeeIds { get; set; } = new();

    }
}
