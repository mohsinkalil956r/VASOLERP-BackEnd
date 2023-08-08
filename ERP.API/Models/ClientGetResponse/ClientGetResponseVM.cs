namespace ERP.API.Models.ClientGetResponse
{
    public class ClientGetResponseVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<ClientContacts.ClientContactPutVM> contacts { get; set; }
    }
}
