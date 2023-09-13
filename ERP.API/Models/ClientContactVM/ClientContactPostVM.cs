using ERP.API.Models.Contacts;

namespace ERP.API.Models.ClientContactVM
{
    public class ClientContactPostVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ContactViewModel Contact { get; set; }
    }
}
