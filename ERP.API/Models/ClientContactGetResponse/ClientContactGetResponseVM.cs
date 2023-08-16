using ERP.API.Models.ClientGetResponse;

namespace ERP.API.Models.ClientContactResponse
{
    public class ClientContactGetResponseVM
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }

        //public ClientGetResponseVM Client { get; set; }
    }
}
