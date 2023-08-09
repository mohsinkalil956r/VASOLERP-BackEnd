using ERP.API.Models.ClientContactResponse;

namespace ERP.API.Models.ClientGetResponse
{
    public class ClientGetResponseVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<ClientContactGetResponseVM> contacts { get; set; }
    }
}
