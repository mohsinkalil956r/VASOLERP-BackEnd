using ERP.API.Models.ClientGetResponse;
using ERP.API.Models.StatusGetResponse;

namespace ERP.API.Models.ProjectGetResponse
{
    public class ProjectGetResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeadLine { get; set; }
        public double Budget { get; set; }
        public ClientGetResponseVM Client { get; set; }

        public StatusGetResponseVM Status { get; set; }
        public List<int> EmployeeIds { get; set; } = new();

    }
}
