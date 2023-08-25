using ERP.API.Models.ClientGetResponse;
using ERP.API.Models.EmployeeGetResponse;

namespace ERP.API.Models.ContactsGetResponseVM
{
    public class ContactsGetResponseVM
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? ReferenceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

    }
}
