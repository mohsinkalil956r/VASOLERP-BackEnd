using ERP.DAL.DB.Entities;

namespace ERP.API.Models.Client
{
    public class ClientPostVM
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }

        //public List<ClientContacts.ClientContactPutVM> contacts { get; set; }

    }
}
