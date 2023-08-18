using ERP.API.Models.ClientContactResponse;
using ERP.API.Models.Projects;

namespace ERP.API.Models.ClientGetResponse
{
    public class ClientGetResponseVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ProjectGetVM Project { get; set; }

    }
}
