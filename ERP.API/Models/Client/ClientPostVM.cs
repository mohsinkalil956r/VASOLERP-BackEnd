using ERP.API.Models.Contacts;
using ERP.DAL.DB.Entities;

namespace ERP.API.Models.Client
{
    public class ClientPostVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ContactsPostVM Contact { get; set; }

    }
}
