using ERP.API.Models.ClientGetResponse;
using ERP.API.Models.EmployeeGetResponse;

namespace ERP.API.Models.ContactsGetResponseVM
{
    public class ContactsGetResponseVM
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        //public EmployeeGetResponseVM Employee { get; set; }
        //public ClientGetResponseVM Client { get; set; }

    }
}
