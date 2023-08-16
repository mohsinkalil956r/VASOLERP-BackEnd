using ERP.API.Models.ClientContactResponse;

namespace ERP.API.Models.ClientGetResponse
{
    public class ClientGetResponseVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }

        // List<ClientContactGetResponseVM> contacts { get; set; }
    }
}
