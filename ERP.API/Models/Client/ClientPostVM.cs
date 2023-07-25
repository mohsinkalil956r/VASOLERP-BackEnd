using ERP.DAL.DB.Entities;

namespace ERP.API.Models.Client
{
    public class ClientPostVM
    {
        public string Name { get; set; }

        public List<ClientContacts.ClientContactPutVM> contacts { get; set; }

    }
}
